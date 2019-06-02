using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Business.Contracts.Services.GeneEditor;
using Business.Contracts.ViewModels.GeneEditor;
using Data.Contracts;
using Data.Contracts.Entities.GeneEditor;
using Data.Contracts.Entities.Identity;
using Shared.Framework.Collections;
using Shared.Framework.Dependency;
using Shared.Framework.Security;

namespace Services.Services.GeneEditor
{
    public class GeneEditorService: IGeneEditorService, IDependency
    {
        private const int DefaultKPlusConcentration = 1;
        private const int DefaultDMSO = 1;
        private const int DefaultOligoLength = 60;
        private const int DefaultOverlappingLength = 20;

        private readonly IUnitOfWork unitOfWork;
        private readonly ICodonOptimizationService codonOptimizationService;
        private readonly IGeneFragmentationService geneFragmentationService;
        private readonly IIdentityStorage identityStorage;

        public GeneEditorService(IUnitOfWork unitOfWork, ICodonOptimizationService codonOptimizationService, 
            IGeneFragmentationService geneFragmentationService, IIdentityStorage identityStorage)
        {
            this.unitOfWork = unitOfWork;
            this.codonOptimizationService = codonOptimizationService;
            this.geneFragmentationService = geneFragmentationService;
            this.identityStorage = identityStorage;
        }

        public async Task<GeneEditorViewModel> GetGeneEditorViewModel()
        {
            GeneEditorViewModel geneEditorViewModel = new GeneEditorViewModel();
            geneEditorViewModel.SelectedInitialSequenceType = InitialSequenceType.ProteinInitialSequence;

            IEnumerable<Organism> organisms = await unitOfWork.GetAll<Organism>().ToListAsync();

            geneEditorViewModel.AllOrganisms = organisms.Select(x => new ListItem() {Id = x.Id, Text = x.Name});
            if (geneEditorViewModel.AllOrganisms.Any())
            {
                ListItem defaultOrganism = geneEditorViewModel.AllOrganisms.First();
                geneEditorViewModel.SelectedOrganismId = defaultOrganism.Id;
            }

            geneEditorViewModel.KPlusConcentration = DefaultKPlusConcentration;
            geneEditorViewModel.DMSO = DefaultDMSO;

            geneEditorViewModel.IsGeneOptimized = false;

            return geneEditorViewModel;
        }

        public GeneEditorViewModel GetGeneEditorViewModel(string geneId)
        {
            Gene gene = unitOfWork.GetById<Gene>(geneId);

            GeneEditorViewModel geneEditorViewModel = new GeneEditorViewModel();

            geneEditorViewModel.IsGeneOptimized = true;
            geneEditorViewModel.Name = gene.Name;
            geneEditorViewModel.GeneId = gene.Id;
            geneEditorViewModel.OptimizedDNASequence = gene.DNASequence;

            Organism organism = unitOfWork.GetById<Organism>(gene.OrganismId);

            geneEditorViewModel.AllOrganisms = new List<ListItem>() { new ListItem { Id = organism.Id, Text = organism.Name } };
            geneEditorViewModel.SelectedOrganismId = organism.Id;
            geneEditorViewModel.SelectedOrganismName = organism.Name;
            
            geneEditorViewModel.KPlusConcentration = gene.KPlusConcentration;
            geneEditorViewModel.DMSO = gene.DMSO;

            var geneFragment = gene.GeneFragments.First();

            geneEditorViewModel.GeneFragmentLength = geneFragment.OligoLength;
            geneEditorViewModel.GeneFragmentOverlappingLength = geneFragment.OverlappingLength;

            return geneEditorViewModel;
        }

        public async Task<GeneEditorViewModel> OptimizeDNASequence(GeneEditorViewModel geneEditorViewModel)
        {
            string optimizedDNASequence = null;

            geneEditorViewModel.InitialSequence = geneEditorViewModel.InitialSequence.Trim().Replace(" ", string.Empty).ToUpper();

            if (geneEditorViewModel.SelectedInitialSequenceType == InitialSequenceType.ProteinInitialSequence)
            {
                optimizedDNASequence = await codonOptimizationService.OptimizeProteinSequence(
                    geneEditorViewModel.InitialSequence, geneEditorViewModel.SelectedOrganismId);
            }
            else if(geneEditorViewModel.SelectedInitialSequenceType == InitialSequenceType.DNAInitialSequence)
            {
                optimizedDNASequence = await codonOptimizationService.OptimizeDNASequence(
                    geneEditorViewModel.InitialSequence, geneEditorViewModel.SelectedOrganismId);
            }

            geneEditorViewModel.OptimizedDNASequence = optimizedDNASequence;

            Gene gene = CreateGene(geneEditorViewModel.Name, geneEditorViewModel.OptimizedDNASequence, 
                geneEditorViewModel.SelectedOrganismId, geneEditorViewModel.KPlusConcentration, geneEditorViewModel.DMSO);

            geneEditorViewModel.GeneId = gene.Id;
            geneEditorViewModel.GeneFragmentLength = DefaultOligoLength;
            geneEditorViewModel.GeneFragmentOverlappingLength = DefaultOverlappingLength;
            geneEditorViewModel.IsGeneOptimized = true;

            Organism organism = unitOfWork.GetById<Organism>(geneEditorViewModel.SelectedOrganismId);
            geneEditorViewModel.SelectedOrganismName = organism.Name;

            return geneEditorViewModel;
        }

        public async Task UpdateGeneFragments(string geneId, string dnaSequence, int oligoLength, int overlappingLength, float kPlusConcentration, float dmso)
        {
            UpdateGene(geneId, kPlusConcentration, dmso);
            await geneFragmentationService.UpdateGeneFragments(geneId, dnaSequence, oligoLength, overlappingLength, kPlusConcentration, dmso);
        }

        public Task DeleteGene(string geneId)
        {
            return unitOfWork.DeleteWhere<Gene>(x => x.Id == geneId);
        }

        private Gene CreateGene(string name, string dnaSequence, string organismId, float kPlusConcentration, float dmso)
        {
            var currentPrincipal = identityStorage.GetPrincipal();
            User currentUser = unitOfWork.GetById<User>(currentPrincipal.UserId);

            Organism organism = unitOfWork.GetById<Organism>(organismId);

            Gene gene = new Gene();
            gene.Name = name;
            gene.DNASequence = dnaSequence;
            gene.OrganismId = organism.Id;
            gene.Organism = organism;
            gene.KPlusConcentration = kPlusConcentration;
            gene.DMSO = dmso;
            gene.UserId = currentUser.Id;
            gene.User = currentUser;

            gene = unitOfWork.InsertOrUpdate(gene);

            unitOfWork.Commit();

            return gene;
        }

        private void UpdateGene(string geneId, float kPlusConcentration, float dmso)
        {
            Gene gene = unitOfWork.GetById<Gene>(geneId);

            var currentPrincipal = identityStorage.GetPrincipal();
            User currentUser = unitOfWork.GetById<User>(currentPrincipal.UserId);
            Organism organism = unitOfWork.GetById<Organism>(gene.OrganismId);

            gene.OrganismId = organism.Id;
            gene.Organism = organism;

            gene.UserId = currentUser.Id;
            gene.User = currentUser;

            gene.KPlusConcentration = kPlusConcentration;
            gene.DMSO = dmso;

            unitOfWork.InsertOrUpdate(gene);

            unitOfWork.Commit();
        }
    }
}