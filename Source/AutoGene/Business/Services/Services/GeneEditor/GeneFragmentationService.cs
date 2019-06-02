using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Contracts.Services.GeneEditor;
using Business.Contracts.ViewModels.GeneEditor;
using Data.Contracts;
using Data.Contracts.Entities.GeneEditor;
using Shared.Framework.Dependency;
using Shared.Framework.Security;

namespace Services.Services.GeneEditor
{
    public class GeneFragmentationService : IGeneFragmentationService, IDependency
    {
        private readonly IUnitOfWork unitOfWork;

        public GeneFragmentationService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<GeneFragmentItemViewModel> GetGeneFragmentItems(string geneId)
        {
            var gene = unitOfWork.GetById<Gene>(geneId);
            IList<GeneFragmentItemViewModel> geneFragments = new List<GeneFragmentItemViewModel>();
            foreach (var geneFragment in gene.GeneFragments.OrderBy(x => x.FragmentNumber))
            {
                geneFragments.Add(new GeneFragmentItemViewModel()
                {
                    Id = geneFragment.Id,
                    GeneId = geneId,
                    FragmentNumber = geneFragment.FragmentNumber,
                    OligoSequence = geneFragment.OligoSequence,
                    OligoLength = geneFragment.OligoLength,
                    OverlappingLength = geneFragment.OverlappingLength,
                    Tm = geneFragment.Tm,
                });
            }

            return geneFragments;
        }

        public void UpdateGeneFragment(GeneFragmentItemViewModel geneFragmentItemViewModel)
        {
            Gene gene = unitOfWork.GetById<Gene>(geneFragmentItemViewModel.GeneId);

            GeneFragment geneFragment = unitOfWork.GetById<GeneFragment>(geneFragmentItemViewModel.Id);
            geneFragment.OligoSequence = geneFragmentItemViewModel.OligoSequence;
            geneFragment.OligoLength = geneFragmentItemViewModel.OligoSequence.Length;
            geneFragment.OverlappingLength = geneFragmentItemViewModel.OverlappingLength;
            geneFragment.Tm = CalculateTm(geneFragment.OligoSequence, gene.KPlusConcentration, gene.DMSO, geneFragment.OverlappingLength);
            geneFragment.Gene = gene;

            unitOfWork.InsertOrUpdate(geneFragment);

            unitOfWork.Commit();
        }
        
        public async Task UpdateGeneFragments(string geneId, string dnaSequence, int oligoLength, int overlappingLength, float kPlusConcentration, float dmso)
        {
//            var sw = Stopwatch.StartNew();

            await unitOfWork.DeleteWhere<GeneFragment>(x => x.GeneId == geneId);

            IList<GeneFragment> geneFragments = new List<GeneFragment>();

            if (dnaSequence.Length <= oligoLength)
            {
                var geneFragment = CreateGeneFragment(geneId, 1, dnaSequence, 1, kPlusConcentration, dmso);
                geneFragments.Add(geneFragment);
            }
            else
            {
                bool isLastGeneFragment = false;
                int fragmentNumber = 1;
                int startIndex = 0;
                int endIndex = oligoLength;
                while (!isLastGeneFragment)
                {
                    int fragmentSequenceLength = endIndex - startIndex;
                    var geneFragmentSequence = dnaSequence.Substring(startIndex, fragmentSequenceLength);

                    var geneFragment = CreateGeneFragment(geneId, fragmentNumber, geneFragmentSequence, overlappingLength, kPlusConcentration, dmso);

                    geneFragments.Add(geneFragment);

                    startIndex = endIndex - overlappingLength;
                    endIndex = startIndex + oligoLength;

                    fragmentNumber++;

                    if (endIndex >= dnaSequence.Length)
                    {

                        fragmentSequenceLength = dnaSequence.Length - startIndex;
                        geneFragmentSequence = dnaSequence.Substring(startIndex, fragmentSequenceLength);

                        geneFragment = CreateGeneFragment(geneId, fragmentNumber, geneFragmentSequence, overlappingLength, kPlusConcentration, dmso);

                        geneFragments.Add(geneFragment);

                        isLastGeneFragment = true;
                    }
                }
            }

            //ReverseComplement(geneFragments);

            await unitOfWork.InsertAll(geneFragments);

            //                        sw.Stop();
            //                        Console.WriteLine($"UpdateGeneFragments: {sw.ElapsedMilliseconds} ms");
        }

        private void ReverseComplement(IList<GeneFragment> geneFragments)
        {
            for (int i = 1; i < geneFragments.Count; i += 2)
            {
                string reversedOligoSequence = new string(geneFragments[i].OligoSequence.ToCharArray().Reverse().ToArray());

                StringBuilder complementOligoSequence = new StringBuilder(reversedOligoSequence.Length);
                foreach (var nucleotide in reversedOligoSequence)
                {
                    if (nucleotide == 'A') complementOligoSequence.Append('T');
                    else if (nucleotide == 'T') complementOligoSequence.Append('A');
                    else if (nucleotide == 'G') complementOligoSequence.Append('C');
                    else if (nucleotide == 'C') complementOligoSequence.Append('G');
                    else complementOligoSequence.Append(nucleotide);
                }

                geneFragments[i].OligoSequence = complementOligoSequence.ToString();
            }

            foreach (GeneFragment geneFragment in geneFragments)
            {
                geneFragment.OligoSequence = new string(geneFragment.OligoSequence.ToCharArray().Reverse().ToArray());
            }
        }

        private GeneFragment CreateGeneFragment(string geneId, int fragmentNumber, string geneFragmentSequence,
            int overlappingLength, float kPlusConcentration, float dmso)
        {
            var geneFragment = new GeneFragment()
            {
                FragmentNumber = fragmentNumber,
                OligoSequence = geneFragmentSequence,
                OligoLength = geneFragmentSequence.Length,
                OverlappingLength = overlappingLength,
                GeneId = geneId,
                CreatedAt = DateTimeOffset.UtcNow
                //Gene = gene,
            };
            geneFragment.Tm = CalculateTm(geneFragmentSequence, kPlusConcentration, dmso, overlappingLength);

            return geneFragment;
        }

        private float CalculateTm(string oligoSequence, float kPlusConcentration, float dmso, int overlappingLength)
        {
            float tm = 0;

            var oligonucleotides = oligoSequence.ToCharArray();
            int countGC = oligonucleotides.Count(x => x == 'G') + oligonucleotides.Count(x => x == 'C');

            if (overlappingLength <= 20)
            {
                int countAT = oligonucleotides.Count(x => x == 'A') + oligonucleotides.Count(x => x == 'T');
                tm = 8 + 2*countAT + 4*countGC;
            }
            else
            {
                tm = (float)(81.5 + 16.6 * Math.Log10(kPlusConcentration) + 41 * countGC / overlappingLength - 500 / overlappingLength - 0.62 * dmso);
            }
            
            return tm;
        }
    }
}