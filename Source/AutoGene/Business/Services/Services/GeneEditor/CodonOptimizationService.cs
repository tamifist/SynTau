using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Business.Contracts.Services.GeneEditor;
using Data.Contracts;
using Data.Contracts.Entities.GeneEditor;
using Shared.Framework.Dependency;
using Shared.Framework.Utilities;

namespace Services.Services.GeneEditor
{
    public class CodonOptimizationService : ICodonOptimizationService, IDependency
    {
        private readonly IUnitOfWork unitOfWork;

        public CodonOptimizationService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<string> OptimizeProteinSequence(string proteinSequence, string organismId)
        {
            if (string.IsNullOrWhiteSpace(proteinSequence) || string.IsNullOrWhiteSpace(organismId))
            {
                throw new ArgumentNullException();
            }

            string optimizedDNASequence = "";

            var allAminoAcids = await unitOfWork.GetAll<AminoAcid>().Include(x => x.Codons).ToListAsync();
            var organism = unitOfWork.GetById<Organism>(organismId);
            foreach (Char aminoCode in proteinSequence)
            {
                string aminoAcidCode = aminoCode.ToString();
                AminoAcid aminoAcid = allAminoAcids.Single(x => x.Code == aminoAcidCode);
                List<CodonUsage> codonUsages = new List<CodonUsage>();
                foreach (Codon codon in aminoAcid.Codons)
                {
                    CodonUsage codonUsage = organism.CodonUsages.SingleOrDefault(x => x.Codon.Triplet == codon.Triplet);
                    if (codonUsage != null)
                    {
                        codonUsages.Add(codonUsage);
                    }
                }

                var maxFrequency = codonUsages.Max(x => x.Frequency);
                CodonUsage maxCodonUsage = codonUsages.First(x => x.Frequency == maxFrequency);
                optimizedDNASequence += maxCodonUsage.Codon.Triplet;
            }
            
            return optimizedDNASequence;
        }

        public async Task<string> OptimizeDNASequence(string dnaSequence, string organismId)
        {
            string proteinSequence = await ConvertDNAToProteinSequence(dnaSequence);
            return await OptimizeProteinSequence(proteinSequence, organismId);
        }

        private async Task<string> ConvertDNAToProteinSequence(string dnaSequence)
        {

            string proteinSequence = "";

            var allAminoAcids = await unitOfWork.GetAll<AminoAcid>().Include(x => x.Codons).ToListAsync();
            IEnumerable<List<char>> dnaTriplets = dnaSequence.ToCharArray().InSetsOf(3);
            foreach (List<char> tripletChars in dnaTriplets)
            {
                string triplet = string.Concat(tripletChars);
                try
                {
                    var aminoAcid = allAminoAcids.Single(x => x.Codons.Any(y => y.Triplet == triplet));
                    proteinSequence += aminoAcid.Code;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Codon: {triplet} doesn't exists");
                }
            }

            return proteinSequence;
        }
    }
}