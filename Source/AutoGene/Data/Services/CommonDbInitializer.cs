using System.Data.Entity;
using System.Linq;
using Data.Contracts.Entities.CycleEditor;
using Data.Contracts.Entities.GeneEditor;
using Data.Contracts.Entities.Identity;
using Data.Services.Extensions;
using Shared.Enum;

namespace Data.Services
{
    public class CommonDbInitializer
    {
        private const string EscherichiaColi = "Escherichia coli (E.coli)";

        private static CommonDbContext commonDbContext;

        public static void SeedInitialData(CommonDbContext context)
        {
            commonDbContext = context;

            SeedRoles();
            SeedAminoAcidsAndCodons();

            commonDbContext.SaveChanges();

            SeedOrganismsAndCodonUsages();

            SeedHardwareFunctions();

            commonDbContext.SaveChanges();

            SeedCef1();

            commonDbContext.SaveChanges();
        }

        private static void SeedOrganismsAndCodonUsages()
        {
            SeedEscherichiaColi();
            //SeedHomoSapiens(context);
        }

        private static void SeedCef1()
        {
            SynthesisCycle cef1 = commonDbContext.SynthesisCycles.Include(x => x.CycleSteps).SingleOrDefault(x => x.UserId == null);
            if (cef1 == null)
            {
                cef1 = new SynthesisCycle();
                cef1.Name = "cef1";
            }

            AddOrUpdateCycleStep(cef1, 1, 10, 3, true, true, true, true, true, true, true, true);
            AddOrUpdateCycleStep(cef1, 2, 9, 20, true, true, true, true, true, true, true, true);
            AddOrUpdateCycleStep(cef1, 3, 2, 5, true, true, true, true, true, true, true, true);
            AddOrUpdateCycleStep(cef1, 4, 1, 3, true, true, true, true, true, true, true, true);
            AddOrUpdateCycleStep(cef1, 5, 28, 3, true, true, true, true, true, true, true, true);
            AddOrUpdateCycleStep(cef1, 6, 90, 5, true, true, true, true, true, true, true, true);
            AddOrUpdateCycleStep(cef1, 7, 19, 4, true, true, true, true, true, true, true, true);
            AddOrUpdateCycleStep(cef1, 8, 90, 3, true, true, true, true, true, true, true, true);
            AddOrUpdateCycleStep(cef1, 9, 19, 3, true, true, true, true, true, true, true, true);
            AddOrUpdateCycleStep(cef1, 10, 90, 3, true, true, true, true, true, true, true, true);
            AddOrUpdateCycleStep(cef1, 11, 19, 3, true, true, true, true, true, true, true, true);
            AddOrUpdateCycleStep(cef1, 12, 9, 1, true, true, true, true, true, true, true, true);
            //AddOrUpdateCycleStep(cef1, 15, 4, 15, true, true, true, true, true, true, true, true);
            AddOrUpdateCycleStep(cef1, 13, 16, 3, true, true, true, true, true, true, true, true);
            AddOrUpdateCycleStep(cef1, 14, 83, 3, true, true, true, true, true, true, true, true);
            AddOrUpdateCycleStep(cef1, 15, 2, 5, true, true, true, true, true, true, true, true);
            AddOrUpdateCycleStep(cef1, 16, 1, 4, true, true, true, true, true, true, true, true);
            AddOrUpdateCycleStep(cef1, 17, 91, 12, true, true, true, true, true, true, true, true);
            AddOrUpdateCycleStep(cef1, 18, 83, 3, true, true, true, true, true, true, true, true);
            //AddOrUpdateCycleStep(cef1, 22, 4, 8, true, true, true, true, true, true, true, true);
            AddOrUpdateCycleStep(cef1, 19, 2, 5, true, true, true, true, true, true, true, true);
            AddOrUpdateCycleStep(cef1, 20, 1, 4, true, true, true, true, true, true, true, true);
            AddOrUpdateCycleStep(cef1, 21, 81, 3, true, true, true, true, true, true, true, true);
            AddOrUpdateCycleStep(cef1, 22, 13, 12, true, true, true, true, true, true, true, true);
            AddOrUpdateCycleStep(cef1, 23, 10, 5, true, true, true, true, true, true, true, true);
            //AddOrUpdateCycleStep(cef1, 28, 4, 15, true, true, true, true, true, true, true, true);
            AddOrUpdateCycleStep(cef1, 24, 2, 6, true, true, true, true, true, true, true, true);
            AddOrUpdateCycleStep(cef1, 25, 1, 4, true, true, true, true, true, true, true, true);
            AddOrUpdateCycleStep(cef1, 26, 15, 10, true, true, true, true, true, true, true, true);
            AddOrUpdateCycleStep(cef1, 27, 34, 5, true, true, true, true, true, true, true, true);
            AddOrUpdateCycleStep(cef1, 28, 15, 10, true, true, true, true, true, true, true, true);
            AddOrUpdateCycleStep(cef1, 29, 2, 5, true, true, true, true, true, true, true, true);
            AddOrUpdateCycleStep(cef1, 30, 15, 10, true, true, true, true, true, true, true, true);
            AddOrUpdateCycleStep(cef1, 31, 2, 5, true, true, true, true, true, true, true, true);
            AddOrUpdateCycleStep(cef1, 32, 1, 4, true, true, true, true, true, true, true, true);
            //AddOrUpdateCycleStep(cef1, 38, 33, 1, true, true, true, true, true, true, true, true);
            AddOrUpdateCycleStep(cef1, 33, 18, 3, true, true, true, true, true, true, true, true);
            AddOrUpdateCycleStep(cef1, 34, 12, 10, true, true, true, true, true, true, true, true);
            AddOrUpdateCycleStep(cef1, 35, 2, 5, true, true, true, true, true, true, true, true);
            AddOrUpdateCycleStep(cef1, 36, 1, 3, true, true, true, true, true, true, true, true);
            //AddOrUpdateCycleStep(cef1, 43, 6, 1, true, true, true, true, true, true, true, true);
            //AddOrUpdateCycleStep(cef1, 44, 5, 1, true, true, true, true, true, true, true, true);
            AddOrUpdateCycleStep(cef1, 37, 82, 3, true, true, true, true, true, true, true, false);
            AddOrUpdateCycleStep(cef1, 38, 14, 10, false, false, true, true, true, true, true, false);
            AddOrUpdateCycleStep(cef1, 39, 34, 1, false, false, true, true, true, true, true, false);
            AddOrUpdateCycleStep(cef1, 40, 14, 10, true, true, true, true, true, true, true, false);
            AddOrUpdateCycleStep(cef1, 41, 34, 1, true, true, true, true, true, true, true, false);
            AddOrUpdateCycleStep(cef1, 42, 14, 10, true, true, true, true, true, true, true, false);
            AddOrUpdateCycleStep(cef1, 43, 34, 1, true, true, true, true, true, true, true, false);
            AddOrUpdateCycleStep(cef1, 44, 14, 10, true, true, true, true, true, true, true, false);
            AddOrUpdateCycleStep(cef1, 45, 34, 1, true, true, true, true, true, true, true, false);
            AddOrUpdateCycleStep(cef1, 46, 14, 10, true, true, true, true, true, true, true, false);
            AddOrUpdateCycleStep(cef1, 47, 34, 1, true, true, true, true, true, true, true, false);
            AddOrUpdateCycleStep(cef1, 48, 14, 10, true, true, true, true, true, true, true, false);
            AddOrUpdateCycleStep(cef1, 49, 34, 8, true, true, true, true, true, true, true, false);
            AddOrUpdateCycleStep(cef1, 50, 12, 10, true, true, true, true, true, true, true, false);
            AddOrUpdateCycleStep(cef1, 51, 34, 8, true, true, true, true, true, true, true, false);
            //AddOrUpdateCycleStep(cef1, 60, 7, 1, true, true, true, true, true, true, true, true);
            AddOrUpdateCycleStep(cef1, 52, 12, 10, true, true, true, true, true, true, true, true);
            AddOrUpdateCycleStep(cef1, 53, 2, 5, true, true, true, true, true, true, true, true);

            commonDbContext.SynthesisCycles.AddIfNotExists(cef1, x => x.UserId == null);
        }

        private static void AddOrUpdateCycleStep(SynthesisCycle synthesisCycle, int stepNumber, int functionNumber, int stepTime,
            bool a, bool g, bool c, bool t, bool five, bool six, bool seven, bool safeStep)
        {
            CycleStep existingCycleStep = synthesisCycle.CycleSteps.FirstOrDefault(x => x.Number == stepNumber);
            if (existingCycleStep != null)
            {
                existingCycleStep.StepTime = stepTime;
                existingCycleStep.A = a;
                existingCycleStep.G = g;
                existingCycleStep.C = c;
                existingCycleStep.T = t;
                existingCycleStep.Five = five;
                existingCycleStep.Six = six;
                existingCycleStep.Seven = seven;
                existingCycleStep.SafeStep = safeStep;
            }
            else
            {
                HardwareFunction hardwareFunction = commonDbContext.HardwareFunctions.Single(x => x.Number == functionNumber);
                CycleStep newCycleStep = new CycleStep();
                newCycleStep.HardwareFunction = hardwareFunction;
                newCycleStep.Number = stepNumber;
                newCycleStep.StepTime = stepTime;
                newCycleStep.A = a;
                newCycleStep.G = g;
                newCycleStep.C = c;
                newCycleStep.T = t;
                newCycleStep.Five = five;
                newCycleStep.Six = six;
                newCycleStep.Seven = seven;
                newCycleStep.SafeStep = safeStep;

                synthesisCycle.CycleSteps.Add(newCycleStep);
            }
        }

        private static void SeedHardwareFunctions()
        {
            AddOrUpdateHardwareFunction(0, "Close all valves", "Закрытие клапанов 00-47", HttpMethodType.POST, HardwareFunctionType.CloseAllValves, "/macros/f00");
            AddOrUpdateHardwareFunction(1, "Block Flush", "16, 36, 41.", HttpMethodType.POST, HardwareFunctionType.CycleStep, "/macros/f01");
            AddOrUpdateHardwareFunction(2, "Reverse Flush", "36, 40, 41.", HttpMethodType.POST, HardwareFunctionType.CycleStep, "/macros/f02");
            AddOrUpdateHardwareFunction(3, "Block To Column", "40, 44.", HttpMethodType.POST, HardwareFunctionType.CycleStep, "/macros/f03");
            AddOrUpdateHardwareFunction(8, "Flush To CLCT", "16, 40, 42, 45.", HttpMethodType.POST, HardwareFunctionType.CycleStep, "/macros/f08");
            AddOrUpdateHardwareFunction(9, "#18 To Column", "02, 17, 40, 44.", HttpMethodType.POST, HardwareFunctionType.CycleStep, "/macros/f09");
            AddOrUpdateHardwareFunction(10, "#18 To Waste", "02, 17, 36", HttpMethodType.POST, HardwareFunctionType.CycleStep, "/macros/f10");
            AddOrUpdateHardwareFunction(11, "#17 To Column", "06, 18, 40, 44", HttpMethodType.POST, HardwareFunctionType.CycleStep, "/macros/f11");
            AddOrUpdateHardwareFunction(12, "#16 To Column", "07, 19, 40, 44", HttpMethodType.POST, HardwareFunctionType.CycleStep, "/macros/f12");
            AddOrUpdateHardwareFunction(13, "#15 To Column", "08, 20, 40, 44", HttpMethodType.POST, HardwareFunctionType.CycleStep, "/macros/f13");
            AddOrUpdateHardwareFunction(14, "#14 To Column", "10, 21, 40, 44", HttpMethodType.POST, HardwareFunctionType.CycleStep, "/macros/f14");
            AddOrUpdateHardwareFunction(15, "#13 To Column", "09, 22, 40, 44", HttpMethodType.POST, HardwareFunctionType.CycleStep, "/macros/f15");
            AddOrUpdateHardwareFunction(16, "Cap Prep", "13", HttpMethodType.POST, HardwareFunctionType.CycleStep, "/macros/f16");
            AddOrUpdateHardwareFunction(18, "#16 To Waste", "07, 19, 36", HttpMethodType.POST, HardwareFunctionType.CycleStep, "/macros/f18");
            AddOrUpdateHardwareFunction(19, "B+TET To Col", "00, 11, 28, 40, 44", HttpMethodType.POST, HardwareFunctionType.BAndTetToCol, "/macros/f19");

            AddOrUpdateHardwareFunction(901, "A+TET To Col", "A", HttpMethodType.POST, HardwareFunctionType.AAndTetToCol, "/macros/f19A");
            AddOrUpdateHardwareFunction(902, "T+TET To Col", "T", HttpMethodType.POST, HardwareFunctionType.TAndTetToCol, "/macros/f19T");
            AddOrUpdateHardwareFunction(903, "C+TET To Col", "C", HttpMethodType.POST, HardwareFunctionType.CAndTetToCol, "/macros/f19C");
            AddOrUpdateHardwareFunction(904, "G+TET To Col", "G", HttpMethodType.POST, HardwareFunctionType.GAndTetToCol, "/macros/f19G");

            AddOrUpdateHardwareFunction(22, "Cap to Col 1", "13, 25, 26, 40, 44", HttpMethodType.POST, HardwareFunctionType.CycleStep, "/macros/f22");
            AddOrUpdateHardwareFunction(25, "#17 To #8", "05, 06, 18, 23", HttpMethodType.POST, HardwareFunctionType.CycleStep, "/macros/f25");
            AddOrUpdateHardwareFunction(26, "#8 To Column", "04, 23, 40, 44", HttpMethodType.POST, HardwareFunctionType.CycleStep, "/macros/f26");
            AddOrUpdateHardwareFunction(27, "#10 To Collect", "14, 24, 40, 42, 45", HttpMethodType.POST, HardwareFunctionType.CycleStep, "/macros/f27");
            AddOrUpdateHardwareFunction(28, "Phos Prep", "00,11", HttpMethodType.POST, HardwareFunctionType.CycleStep, "/macros/f28");
            AddOrUpdateHardwareFunction(29, "Flush To #8", "05, 16, 23", HttpMethodType.POST, HardwareFunctionType.CycleStep, "/macros/f29");
            AddOrUpdateHardwareFunction(30, "#17 To Waste", "06, 18, 36", HttpMethodType.POST, HardwareFunctionType.CycleStep, "/macros/f30");
            AddOrUpdateHardwareFunction(34, "Flush To Waste", "16, 40, 44", HttpMethodType.POST, HardwareFunctionType.CycleStep, "/macros/f34");
            AddOrUpdateHardwareFunction(41, "#8 Vent", "5", HttpMethodType.POST, HardwareFunctionType.CycleStep, "/macros/f41");
            AddOrUpdateHardwareFunction(42, "#10 Vent", "01, 15", HttpMethodType.POST, HardwareFunctionType.CycleStep, "/macros/f42");
            AddOrUpdateHardwareFunction(43, "#18 Purge", "02, 03", HttpMethodType.POST, HardwareFunctionType.CycleStep, "/macros/f43");
            AddOrUpdateHardwareFunction(44, "Phos Purge", "11, 12", HttpMethodType.POST, HardwareFunctionType.CycleStep, "/macros/f44");
            AddOrUpdateHardwareFunction(51, "Tet Purge", "00, 01", HttpMethodType.POST, HardwareFunctionType.CycleStep, "/macros/f51");
            AddOrUpdateHardwareFunction(52, "A To Waste", "11, 35, 36", HttpMethodType.POST, HardwareFunctionType.CycleStep, "/macros/f52");
            AddOrUpdateHardwareFunction(53, "G To Waste", "11, 34, 36", HttpMethodType.POST, HardwareFunctionType.CycleStep, "/macros/f53");
            AddOrUpdateHardwareFunction(54, "C To Waste", "11, 33, 36", HttpMethodType.POST, HardwareFunctionType.CycleStep, "/macros/f54");
            AddOrUpdateHardwareFunction(55, "T To Waste", "11, 32, 36", HttpMethodType.POST, HardwareFunctionType.CycleStep, "/macros/f55");
            AddOrUpdateHardwareFunction(56, "#5 To Waste", "11, 31, 36", HttpMethodType.POST, HardwareFunctionType.CycleStep, "/macros/f56");
            AddOrUpdateHardwareFunction(57, "#6 To Waste", "11, 30, 36", HttpMethodType.POST, HardwareFunctionType.CycleStep, "/macros/f57");
            AddOrUpdateHardwareFunction(58, "#7 To Waste", "11, 29, 36", HttpMethodType.POST, HardwareFunctionType.CycleStep, "/macros/f58");
            AddOrUpdateHardwareFunction(59, "Cap A To Waste", "13, 26, 36", HttpMethodType.POST, HardwareFunctionType.CycleStep, "/macros/f59");
            AddOrUpdateHardwareFunction(60, "Cap B To Waste", "13, 25, 36", HttpMethodType.POST, HardwareFunctionType.CycleStep, "/macros/f60");
            AddOrUpdateHardwareFunction(61, "TET To Waste", "00, 28, 36", HttpMethodType.POST, HardwareFunctionType.CycleStep, "/macros/f61");
            AddOrUpdateHardwareFunction(62, "Flush To A", "12, 16, 35", HttpMethodType.POST, HardwareFunctionType.CycleStep, "/macros/f62");
            AddOrUpdateHardwareFunction(63, "Flush To G", "12, 16, 34", HttpMethodType.POST, HardwareFunctionType.CycleStep, "/macros/f63");
            AddOrUpdateHardwareFunction(64, "Flush To C", "12, 16, 33", HttpMethodType.POST, HardwareFunctionType.CycleStep, "/macros/f64");
            AddOrUpdateHardwareFunction(65, "Flush To T", "12, 16, 32", HttpMethodType.POST, HardwareFunctionType.CycleStep, "/macros/f65");
            AddOrUpdateHardwareFunction(66, "Flush To #5", "12, 16, 31", HttpMethodType.POST, HardwareFunctionType.CycleStep, "/macros/f66");
            AddOrUpdateHardwareFunction(67, "Flush To #6", "12, 16, 30", HttpMethodType.POST, HardwareFunctionType.CycleStep, "/macros/f67");
            AddOrUpdateHardwareFunction(68, "Flush To #7", "12, 16, 29", HttpMethodType.POST, HardwareFunctionType.CycleStep, "/macros/f68");
            AddOrUpdateHardwareFunction(69, "Flush To TET", "01, 16, 28", HttpMethodType.POST, HardwareFunctionType.CycleStep, "/macros/f69");
            AddOrUpdateHardwareFunction(70, "Flush To #18", "03, 16, 17", HttpMethodType.POST, HardwareFunctionType.CycleStep, "/macros/f70");
            AddOrUpdateHardwareFunction(71, "#18 To A", "02, 12, 17, 35", HttpMethodType.POST, HardwareFunctionType.CycleStep, "/macros/f71");
            AddOrUpdateHardwareFunction(72, "#18 To G", "02, 12, 17, 34", HttpMethodType.POST, HardwareFunctionType.CycleStep, "/macros/f72");
            AddOrUpdateHardwareFunction(73, "#18 To C", "02, 12, 17, 33", HttpMethodType.POST, HardwareFunctionType.CycleStep, "/macros/f73");
            AddOrUpdateHardwareFunction(74, "#18 To T", "02, 12, 17, 32", HttpMethodType.POST, HardwareFunctionType.CycleStep, "/macros/f74");
            AddOrUpdateHardwareFunction(75, "#18 To #5", "02, 12, 17, 31", HttpMethodType.POST, HardwareFunctionType.CycleStep, "/macros/f75");
            AddOrUpdateHardwareFunction(76, "#18 To #6", "02, 12, 17, 30", HttpMethodType.POST, HardwareFunctionType.CycleStep, "/macros/f76");
            AddOrUpdateHardwareFunction(77, "#18 To #7", "02, 12, 17, 39", HttpMethodType.POST, HardwareFunctionType.CycleStep, "/macros/f77");
            AddOrUpdateHardwareFunction(78, "#18 To TET", "01, 02, 17, 28", HttpMethodType.POST, HardwareFunctionType.CycleStep, "/macros/f78");
            AddOrUpdateHardwareFunction(79, "#8 To Waste", "04, 23, 36", HttpMethodType.POST, HardwareFunctionType.CycleStep, "/macros/f79");
            AddOrUpdateHardwareFunction(80, "#10 To Waste", "14, 24, 36", HttpMethodType.POST, HardwareFunctionType.CycleStep, "/macros/f80");
            AddOrUpdateHardwareFunction(81, "#15 To Waste", "08, 20, 36", HttpMethodType.POST, HardwareFunctionType.CycleStep, "/macros/f81");
            AddOrUpdateHardwareFunction(82, "#14 To Waste", "10, 21, 36", HttpMethodType.POST, HardwareFunctionType.CycleStep, "/macros/f82");
            AddOrUpdateHardwareFunction(83, "#13 To Waste", "09, 22, 36", HttpMethodType.POST, HardwareFunctionType.CycleStep, "/macros/f83");
            AddOrUpdateHardwareFunction(84, "#18 To #14+#15", "02, 17, 20, 21", HttpMethodType.POST, HardwareFunctionType.CycleStep, "/macros/f84");
            AddOrUpdateHardwareFunction(85, "Flush-#14+#15", "16, 20, 21", HttpMethodType.POST, HardwareFunctionType.CycleStep, "/macros/f85");
            AddOrUpdateHardwareFunction(86, "#18 To #11+#12", "02, 17, 25, 26", HttpMethodType.POST, HardwareFunctionType.CycleStep, "/macros/f86");
            AddOrUpdateHardwareFunction(87, "Flush- #11+#12", "16, 25, 26", HttpMethodType.POST, HardwareFunctionType.CycleStep, "/macros/f87");
            AddOrUpdateHardwareFunction(88, "#18 To #10", "02, 17, 24", HttpMethodType.POST, HardwareFunctionType.CycleStep, "/macros/f88");
            AddOrUpdateHardwareFunction(89, "Flush To #10", "16, 24", HttpMethodType.POST, HardwareFunctionType.CycleStep, "/macros/f89");
            AddOrUpdateHardwareFunction(90, "Tet To Column", "00, 28, 40, 44", HttpMethodType.POST, HardwareFunctionType.CycleStep, "/macros/f90");
            AddOrUpdateHardwareFunction(91, "Cap To Column", "13, 25, 26, 40, 44", HttpMethodType.POST, HardwareFunctionType.CycleStep, "/macros/f91");

            //basic functions
            AddOrUpdateHardwareFunction(1001, "u_strk_on", "Напряжение переключения (30 V) на платы управления, установить", HttpMethodType.POST, HardwareFunctionType.StrikeOn, "/macros/u_strk_on");
            AddOrUpdateHardwareFunction(1002, "u_strk_off", "Напряжение переключения (30 V) на платы управления, снять", HttpMethodType.POST, HardwareFunctionType.StrikeOff, "/macros/u_strk_off");
            AddOrUpdateHardwareFunction(1003, "u_hold_on", "Напряжение удержания (10 V) на платы управления, установить", HttpMethodType.POST, HardwareFunctionType.HoldOn, "/macros/u_hold_on");
            AddOrUpdateHardwareFunction(1004, "u_hold_off", "Напряжение удержания (10 V) на платы управления, снять", HttpMethodType.POST, HardwareFunctionType.HoldOff, "/macros/u_hold_off");

            AddOrUpdateHardwareFunction(1005, "valve00", "Открыть ключевой транзистор клапана №00", HttpMethodType.POST, HardwareFunctionType.Valve, "/macros/valve00_on");
            AddOrUpdateHardwareFunction(1006, "valve01", "Открыть ключевой транзистор клапана №01", HttpMethodType.POST, HardwareFunctionType.Valve, "/macros/valve01_on");            
            AddOrUpdateHardwareFunction(1007, "valve02", "Открыть ключевой транзистор клапана №02", HttpMethodType.POST, HardwareFunctionType.Valve, "/macros/valve02_on");
            AddOrUpdateHardwareFunction(1008, "valve03", "Открыть ключевой транзистор клапана №03", HttpMethodType.POST, HardwareFunctionType.Valve, "/macros/valve03_on");
            AddOrUpdateHardwareFunction(1009, "valve04", "Открыть ключевой транзистор клапана №04", HttpMethodType.POST, HardwareFunctionType.Valve, "/macros/valve04_on");
            AddOrUpdateHardwareFunction(1010, "valve05", "Открыть ключевой транзистор клапана №05", HttpMethodType.POST, HardwareFunctionType.Valve, "/macros/valve05_on");
            AddOrUpdateHardwareFunction(1011, "valve06", "Открыть ключевой транзистор клапана №06", HttpMethodType.POST, HardwareFunctionType.Valve, "/macros/valve06_on");
            AddOrUpdateHardwareFunction(1012, "valve07", "Открыть ключевой транзистор клапана №07", HttpMethodType.POST, HardwareFunctionType.Valve, "/macros/valve07_on");
            AddOrUpdateHardwareFunction(1013, "valve08", "Открыть ключевой транзистор клапана №08", HttpMethodType.POST, HardwareFunctionType.Valve, "/macros/valve08_on");
            AddOrUpdateHardwareFunction(1014, "valve09", "Открыть ключевой транзистор клапана №09", HttpMethodType.POST, HardwareFunctionType.Valve, "/macros/valve09_on");
            AddOrUpdateHardwareFunction(1015, "valve10", "Открыть ключевой транзистор клапана №10", HttpMethodType.POST, HardwareFunctionType.Valve, "/macros/valve10_on");
            AddOrUpdateHardwareFunction(1016, "valve11", "Открыть ключевой транзистор клапана №11", HttpMethodType.POST, HardwareFunctionType.Valve, "/macros/valve11_on");
            AddOrUpdateHardwareFunction(1017, "valve12", "Открыть ключевой транзистор клапана №12", HttpMethodType.POST, HardwareFunctionType.Valve, "/macros/valve12_on");
            AddOrUpdateHardwareFunction(1018, "valve13", "Открыть ключевой транзистор клапана №13", HttpMethodType.POST, HardwareFunctionType.Valve, "/macros/valve13_on");
            AddOrUpdateHardwareFunction(1019, "valve14", "Открыть ключевой транзистор клапана №14", HttpMethodType.POST, HardwareFunctionType.Valve, "/macros/valve14_on");
            AddOrUpdateHardwareFunction(1020, "valve15", "Открыть ключевой транзистор клапана №15", HttpMethodType.POST, HardwareFunctionType.Valve, "/macros/valve15_on");
            AddOrUpdateHardwareFunction(1021, "valve16", "Открыть ключевой транзистор клапана №16", HttpMethodType.POST, HardwareFunctionType.Valve, "/macros/valve16_on");
            AddOrUpdateHardwareFunction(1022, "valve17", "Открыть ключевой транзистор клапана №17", HttpMethodType.POST, HardwareFunctionType.Valve, "/macros/valve17_on");
            AddOrUpdateHardwareFunction(1023, "valve18", "Открыть ключевой транзистор клапана №18", HttpMethodType.POST, HardwareFunctionType.Valve, "/macros/valve18_on");
            AddOrUpdateHardwareFunction(1024, "valve19", "Открыть ключевой транзистор клапана №19", HttpMethodType.POST, HardwareFunctionType.Valve, "/macros/valve19_on");
            AddOrUpdateHardwareFunction(1025, "valve20", "Открыть ключевой транзистор клапана №20", HttpMethodType.POST, HardwareFunctionType.Valve, "/macros/valve20_on");
            AddOrUpdateHardwareFunction(1026, "valve21", "Открыть ключевой транзистор клапана №21", HttpMethodType.POST, HardwareFunctionType.Valve, "/macros/valve21_on");
            AddOrUpdateHardwareFunction(1027, "valve22", "Открыть ключевой транзистор клапана №22", HttpMethodType.POST, HardwareFunctionType.Valve, "/macros/valve22_on");
            AddOrUpdateHardwareFunction(1028, "valve23", "Открыть ключевой транзистор клапана №23", HttpMethodType.POST, HardwareFunctionType.Valve, "/macros/valve23_on");
            AddOrUpdateHardwareFunction(1029, "valve24", "Открыть ключевой транзистор клапана №24", HttpMethodType.POST, HardwareFunctionType.Valve, "/macros/valve24_on");
            AddOrUpdateHardwareFunction(1030, "valve25", "Открыть ключевой транзистор клапана №25", HttpMethodType.POST, HardwareFunctionType.Valve, "/macros/valve25_on");
            AddOrUpdateHardwareFunction(1031, "valve26", "Открыть ключевой транзистор клапана №26", HttpMethodType.POST, HardwareFunctionType.Valve, "/macros/valve26_on");
            AddOrUpdateHardwareFunction(1032, "valve27", "Открыть ключевой транзистор клапана №27", HttpMethodType.POST, HardwareFunctionType.Valve, "/macros/valve27_on");
            AddOrUpdateHardwareFunction(1033, "valve28", "Открыть ключевой транзистор клапана №28", HttpMethodType.POST, HardwareFunctionType.Valve, "/macros/valve28_on");
            AddOrUpdateHardwareFunction(1034, "valve29", "Открыть ключевой транзистор клапана №29", HttpMethodType.POST, HardwareFunctionType.Valve, "/macros/valve29_on");
            AddOrUpdateHardwareFunction(1035, "valve30", "Открыть ключевой транзистор клапана №30", HttpMethodType.POST, HardwareFunctionType.Valve, "/macros/valve30_on");
            AddOrUpdateHardwareFunction(1036, "valve31", "Открыть ключевой транзистор клапана №31", HttpMethodType.POST, HardwareFunctionType.Valve, "/macros/valve31_on");
            AddOrUpdateHardwareFunction(1037, "valve32", "Открыть ключевой транзистор клапана №32", HttpMethodType.POST, HardwareFunctionType.Valve, "/macros/valve32_on");
            AddOrUpdateHardwareFunction(1038, "valve33", "Открыть ключевой транзистор клапана №33", HttpMethodType.POST, HardwareFunctionType.Valve, "/macros/valve33_on");
            AddOrUpdateHardwareFunction(1039, "valve34", "Открыть ключевой транзистор клапана №34", HttpMethodType.POST, HardwareFunctionType.Valve, "/macros/valve34_on");
            AddOrUpdateHardwareFunction(1040, "valve35", "Открыть ключевой транзистор клапана №35", HttpMethodType.POST, HardwareFunctionType.Valve, "/macros/valve35_on");
            AddOrUpdateHardwareFunction(1041, "valve36", "Открыть ключевой транзистор клапана №36", HttpMethodType.POST, HardwareFunctionType.Valve, "/macros/valve36_on");
            AddOrUpdateHardwareFunction(1042, "valve37", "Открыть ключевой транзистор клапана №37", HttpMethodType.POST, HardwareFunctionType.Valve, "/macros/valve37_on");
            AddOrUpdateHardwareFunction(1043, "valve38", "Открыть ключевой транзистор клапана №38", HttpMethodType.POST, HardwareFunctionType.Valve, "/macros/valve38_on");
            AddOrUpdateHardwareFunction(1044, "valve39", "Открыть ключевой транзистор клапана №39", HttpMethodType.POST, HardwareFunctionType.Valve, "/macros/valve39_on");
            AddOrUpdateHardwareFunction(1045, "valve40", "Открыть ключевой транзистор клапана №40", HttpMethodType.POST, HardwareFunctionType.Valve, "/macros/valve40_on");
            AddOrUpdateHardwareFunction(1046, "valve41", "Открыть ключевой транзистор клапана №41", HttpMethodType.POST, HardwareFunctionType.Valve, "/macros/valve41_on");
            AddOrUpdateHardwareFunction(1047, "valve42", "Открыть ключевой транзистор клапана №42", HttpMethodType.POST, HardwareFunctionType.Valve, "/macros/valve42_on");
            AddOrUpdateHardwareFunction(1048, "valve43", "Открыть ключевой транзистор клапана №43", HttpMethodType.POST, HardwareFunctionType.Valve, "/macros/valve43_on");
            AddOrUpdateHardwareFunction(1049, "valve44", "Открыть ключевой транзистор клапана №44", HttpMethodType.POST, HardwareFunctionType.Valve, "/macros/valve44_on");
            AddOrUpdateHardwareFunction(1050, "valve45", "Открыть ключевой транзистор клапана №45", HttpMethodType.POST, HardwareFunctionType.Valve, "/macros/valve45_on");
            AddOrUpdateHardwareFunction(1051, "valve46", "Открыть ключевой транзистор клапана №46", HttpMethodType.POST, HardwareFunctionType.Valve, "/macros/valve46_on");
            AddOrUpdateHardwareFunction(1052, "valve47", "Открыть ключевой транзистор клапана №47", HttpMethodType.POST, HardwareFunctionType.Valve, "/macros/valve47_on");

            AddOrUpdateHardwareFunction(2001, "syn_channel_01", "Open channel #1", HttpMethodType.POST, HardwareFunctionType.ActivateChannel, "/macros/syn_channel_01");
            AddOrUpdateHardwareFunction(2002, "syn_channel_02", "Open channel #2", HttpMethodType.POST, HardwareFunctionType.ActivateChannel, "/macros/syn_channel_02");
            AddOrUpdateHardwareFunction(2003, "syn_channel_03", "Open channel #3", HttpMethodType.POST, HardwareFunctionType.ActivateChannel, "/macros/syn_channel_03");
            AddOrUpdateHardwareFunction(2004, "syn_channel_04", "Open channel #4", HttpMethodType.POST, HardwareFunctionType.ActivateChannel, "/macros/syn_channel_04");

            AddOrUpdateHardwareFunction(3001, "set_syringe_init_position", "Init syringe pump", HttpMethodType.POST, HardwareFunctionType.SyringePumpInit, "/macros/set_syringe_init_position/{0},{1}");
            AddOrUpdateHardwareFunction(3002, "set_syringe_fin_position", "Fin syringe pump", HttpMethodType.POST, HardwareFunctionType.SyringePumpFin, "/macros/set_syringe_fin_position/{0},{1}");
            AddOrUpdateHardwareFunction(3003, "draw_syringe", "Fin syringe pump", HttpMethodType.POST, HardwareFunctionType.SyringePumpDraw, "/macros/draw_syringe/{0},{1},{2}");

            AddOrUpdateHardwareFunction(3101, "tray_out", "Tray Out", HttpMethodType.POST, HardwareFunctionType.TrayOut, "/macros/tray_out");
            AddOrUpdateHardwareFunction(3102, "tray_in", "Tray In", HttpMethodType.POST, HardwareFunctionType.TrayIn, "/macros/tray_in");
            AddOrUpdateHardwareFunction(3103, "tray_light_on", "Tray light on", HttpMethodType.POST, HardwareFunctionType.TrayLightOn, "/macros/tray_light_on");
            AddOrUpdateHardwareFunction(3104, "tray_light_off", "Tray light off", HttpMethodType.POST, HardwareFunctionType.TrayLightOff, "/macros/tray_light_off");

            AddOrUpdateHardwareFunction(3201, "gs_valve01_open", "gs_valve01_open", HttpMethodType.POST, HardwareFunctionType.GSValve, "/macros/gs_valve01_open");
            AddOrUpdateHardwareFunction(3202, "gs_valve01_close", "gs_valve01_close", HttpMethodType.POST, HardwareFunctionType.GSValve, "/macros/gs_valve01_close");
            AddOrUpdateHardwareFunction(3203, "gs_valve02_open", "gs_valve02_open", HttpMethodType.POST, HardwareFunctionType.GSValve, "/macros/gs_valve02_open");
            AddOrUpdateHardwareFunction(3204, "gs_valve02_close", "gs_valve02_close", HttpMethodType.POST, HardwareFunctionType.GSValve, "/macros/gs_valve02_close");
            AddOrUpdateHardwareFunction(3205, "gs_valve03_open", "gs_valve03_open", HttpMethodType.POST, HardwareFunctionType.GSValve, "/macros/gs_valve03_open");
            AddOrUpdateHardwareFunction(3206, "gs_valve03_close", "gs_valve03_close", HttpMethodType.POST, HardwareFunctionType.GSValve, "/macros/gs_valve03_close");
            AddOrUpdateHardwareFunction(3207, "gs_valve04_open", "gs_valve04_open", HttpMethodType.POST, HardwareFunctionType.GSValve, "/macros/gs_valve04_open");
            AddOrUpdateHardwareFunction(3208, "gs_valve04_close", "gs_valve04_close", HttpMethodType.POST, HardwareFunctionType.GSValve, "/macros/gs_valve04_close");
        }

        private static void AddOrUpdateHardwareFunction(int number, string name, string description, HttpMethodType httpMethodType, 
            HardwareFunctionType functionType, string apiUrl)
        {
            HardwareFunction existingHardwareFunction = commonDbContext.HardwareFunctions.SingleOrDefault(x => x.Number == number);
            if (existingHardwareFunction != null)
            {
                existingHardwareFunction.Name = name;
                existingHardwareFunction.Description = description;
                existingHardwareFunction.HttpMethodType = httpMethodType;
                existingHardwareFunction.FunctionType = functionType;
                existingHardwareFunction.ApiUrl = apiUrl;
            }
            else
            {
                HardwareFunction hardwareFunction = new HardwareFunction();
                hardwareFunction.Number = number;
                hardwareFunction.Name = name;
                hardwareFunction.Description = description;
                hardwareFunction.HttpMethodType = httpMethodType;
                hardwareFunction.FunctionType = functionType;
                hardwareFunction.ApiUrl = apiUrl;

                commonDbContext.HardwareFunctions.Add(hardwareFunction);
            }
        }

        private static void SeedHomoSapiens(CommonDbContext context)
        {
            Organism homoSapiens = new Organism();
            homoSapiens.Name = "Homo sapiens";

            context.Organisms.AddIfNotExists(homoSapiens, x => x.Name == homoSapiens.Name);
        }
        
        private static void SeedEscherichiaColi()
        {
            Organism escherichiaColi = commonDbContext.Organisms.Include(x => x.CodonUsages).SingleOrDefault(x => x.Name == EscherichiaColi);
            if (escherichiaColi == null)
            {
                escherichiaColi = new Organism();
                escherichiaColi.Name = "Escherichia coli (E.coli)";
            }

            AddOrUpdateCodonUsage(escherichiaColi, "TTT", 22.2f);
            AddOrUpdateCodonUsage(escherichiaColi, "TTC", 16.5f);
            AddOrUpdateCodonUsage(escherichiaColi, "TTA", 13.8f);
            AddOrUpdateCodonUsage(escherichiaColi, "TTG", 13.6f);

            AddOrUpdateCodonUsage(escherichiaColi, "CTT", 11.0f);
            AddOrUpdateCodonUsage(escherichiaColi, "CTC", 11.1f);
            AddOrUpdateCodonUsage(escherichiaColi, "CTA", 3.8f);
            AddOrUpdateCodonUsage(escherichiaColi, "CTG", 53.1f);

            AddOrUpdateCodonUsage(escherichiaColi, "ATT", 30.4f);
            AddOrUpdateCodonUsage(escherichiaColi, "ATC", 25.2f);
            AddOrUpdateCodonUsage(escherichiaColi, "ATA", 4.2f);
            AddOrUpdateCodonUsage(escherichiaColi, "ATG", 27.8f);

            AddOrUpdateCodonUsage(escherichiaColi, "GTT", 18.2f);
            AddOrUpdateCodonUsage(escherichiaColi, "GTC", 15.3f);
            AddOrUpdateCodonUsage(escherichiaColi, "GTA", 10.9f);
            AddOrUpdateCodonUsage(escherichiaColi, "GTG", 26.3f);

            AddOrUpdateCodonUsage(escherichiaColi, "TCT", 8.4f);
            AddOrUpdateCodonUsage(escherichiaColi, "TCC", 8.6f);
            AddOrUpdateCodonUsage(escherichiaColi, "TCA", 7.0f);
            AddOrUpdateCodonUsage(escherichiaColi, "TCG", 8.9f);

            AddOrUpdateCodonUsage(escherichiaColi, "CCT", 7.0f);
            AddOrUpdateCodonUsage(escherichiaColi, "CCC", 5.5f);
            AddOrUpdateCodonUsage(escherichiaColi, "CCA", 8.4f);
            AddOrUpdateCodonUsage(escherichiaColi, "CCG", 23.4f);

            AddOrUpdateCodonUsage(escherichiaColi, "ACT", 8.8f);
            AddOrUpdateCodonUsage(escherichiaColi, "ACC", 23.5f);
            AddOrUpdateCodonUsage(escherichiaColi, "ACA", 6.9f);
            AddOrUpdateCodonUsage(escherichiaColi, "ACG", 14.4f);

            AddOrUpdateCodonUsage(escherichiaColi, "GCT", 15.2f);
            AddOrUpdateCodonUsage(escherichiaColi, "GCC", 25.7f);
            AddOrUpdateCodonUsage(escherichiaColi, "GCA", 20.1f);
            AddOrUpdateCodonUsage(escherichiaColi, "GCG", 33.9f);

            AddOrUpdateCodonUsage(escherichiaColi, "TAT", 16.1f);
            AddOrUpdateCodonUsage(escherichiaColi, "TAC", 12.2f);
            AddOrUpdateCodonUsage(escherichiaColi, "TAA", 2.0f);
            AddOrUpdateCodonUsage(escherichiaColi, "TAG", 0.2f);

            AddOrUpdateCodonUsage(escherichiaColi, "CAT", 13.0f);
            AddOrUpdateCodonUsage(escherichiaColi, "CAC", 9.8f);
            AddOrUpdateCodonUsage(escherichiaColi, "CAA", 15.4f);
            AddOrUpdateCodonUsage(escherichiaColi, "CAG", 29.0f);

            AddOrUpdateCodonUsage(escherichiaColi, "AAT", 17.6f);
            AddOrUpdateCodonUsage(escherichiaColi, "AAC", 21.6f);
            AddOrUpdateCodonUsage(escherichiaColi, "AAA", 33.6f);
            AddOrUpdateCodonUsage(escherichiaColi, "AAG", 10.3f);

            AddOrUpdateCodonUsage(escherichiaColi, "GAT", 32.2f);
            AddOrUpdateCodonUsage(escherichiaColi, "GAC", 19.1f);
            AddOrUpdateCodonUsage(escherichiaColi, "GAA", 39.7f);
            AddOrUpdateCodonUsage(escherichiaColi, "GAG", 18.0f);

            AddOrUpdateCodonUsage(escherichiaColi, "TGT", 5.1f);
            AddOrUpdateCodonUsage(escherichiaColi, "TGC", 6.4f);
            AddOrUpdateCodonUsage(escherichiaColi, "TGA", 0.9f);
            AddOrUpdateCodonUsage(escherichiaColi, "TGG", 15.2f);

            AddOrUpdateCodonUsage(escherichiaColi, "CGT", 21.0f);
            AddOrUpdateCodonUsage(escherichiaColi, "CGC", 22.3f);
            AddOrUpdateCodonUsage(escherichiaColi, "CGA", 3.5f);
            AddOrUpdateCodonUsage(escherichiaColi, "CGG", 5.4f);

            AddOrUpdateCodonUsage(escherichiaColi, "AGT", 8.7f);
            AddOrUpdateCodonUsage(escherichiaColi, "AGC", 16.1f);
            AddOrUpdateCodonUsage(escherichiaColi, "AGA", 2.0f);
            AddOrUpdateCodonUsage(escherichiaColi, "AGG", 1.1f);

            AddOrUpdateCodonUsage(escherichiaColi, "GGT", 24.7f);
            AddOrUpdateCodonUsage(escherichiaColi, "GGC", 29.8f);
            AddOrUpdateCodonUsage(escherichiaColi, "GGA", 7.9f);
            AddOrUpdateCodonUsage(escherichiaColi, "GGG", 11.0f);

            commonDbContext.Organisms.AddIfNotExists(escherichiaColi, x => x.Name == escherichiaColi.Name);
        }

        private static void AddOrUpdateCodonUsage(Organism organism, string triplet, float frequency)
        {
            CodonUsage codonUsage = organism.CodonUsages.SingleOrDefault(x => x.Codon.Triplet == triplet);
            if (codonUsage != null)
            {
                codonUsage.Frequency = frequency;
            }
            else
            {
                Codon codon = commonDbContext.Codons.Single(x => x.Triplet == triplet);
                organism.CodonUsages.Add(new CodonUsage { Codon = codon, Frequency = frequency });
            }
        }

        private static void SeedAminoAcidsAndCodons()
        {
            // 1. Alanine
            AminoAcid alanine = new AminoAcid();
            alanine.Name = "Alanine";
            alanine.Code = "A";
            alanine.Codons.Add(new Codon() { Triplet = "GCT" });
            alanine.Codons.Add(new Codon() { Triplet = "GCC" });
            alanine.Codons.Add(new Codon() { Triplet = "GCA" });
            alanine.Codons.Add(new Codon() { Triplet = "GCG" });
            commonDbContext.AminoAcids.AddIfNotExists(alanine, x => x.Code == alanine.Code);

            // 2. Arginine
            AminoAcid arginine = new AminoAcid();
            arginine.Name = "Arginine";
            arginine.Code = "R";
            arginine.Codons.Add(new Codon() { Triplet = "CGT" });
            arginine.Codons.Add(new Codon() { Triplet = "CGC" });
            arginine.Codons.Add(new Codon() { Triplet = "CGA" });
            arginine.Codons.Add(new Codon() { Triplet = "CGG" });
            arginine.Codons.Add(new Codon() { Triplet = "AGA" });
            arginine.Codons.Add(new Codon() { Triplet = "AGG" });
            commonDbContext.AminoAcids.AddIfNotExists(arginine, x => x.Code == arginine.Code);

            // 3. Asparagine
            AminoAcid asparagine = new AminoAcid();
            asparagine.Name = "Asparagine";
            asparagine.Code = "N";
            asparagine.Codons.Add(new Codon() { Triplet = "AAT" });
            asparagine.Codons.Add(new Codon() { Triplet = "AAC" });
            commonDbContext.AminoAcids.AddIfNotExists(asparagine, x => x.Code == asparagine.Code);

            // 4. Aspartic acid
            AminoAcid asparticAcid = new AminoAcid();
            asparticAcid.Name = "Aspartic acid";
            asparticAcid.Code = "D";
            asparticAcid.Codons.Add(new Codon() { Triplet = "GAT" });
            asparticAcid.Codons.Add(new Codon() { Triplet = "GAC" });
            commonDbContext.AminoAcids.AddIfNotExists(asparticAcid, x => x.Code == asparticAcid.Code);

            // 5. Phenylalanine
            AminoAcid cysteine = new AminoAcid();
            cysteine.Name = "Cysteine";
            cysteine.Code = "C";
            cysteine.Codons.Add(new Codon() { Triplet = "TGT" });
            cysteine.Codons.Add(new Codon() { Triplet = "TGC" });
            commonDbContext.AminoAcids.AddIfNotExists(cysteine, x => x.Code == cysteine.Code);

            // 6. Phenylalanine
            AminoAcid glutamine = new AminoAcid();
            glutamine.Name = "Glutamine";
            glutamine.Code = "Q";
            glutamine.Codons.Add(new Codon() { Triplet = "CAA" });
            glutamine.Codons.Add(new Codon() { Triplet = "CAG" });
            commonDbContext.AminoAcids.AddIfNotExists(glutamine, x => x.Code == glutamine.Code);

            // 7. Glutamic acid
            AminoAcid glutamicAcid = new AminoAcid();
            glutamicAcid.Name = "Glutamic acid";
            glutamicAcid.Code = "E";
            glutamicAcid.Codons.Add(new Codon() { Triplet = "GAA" });
            glutamicAcid.Codons.Add(new Codon() { Triplet = "GAG" });
            commonDbContext.AminoAcids.AddIfNotExists(glutamicAcid, x => x.Code == glutamicAcid.Code);

            // 8. Glycine
            AminoAcid glycine = new AminoAcid();
            glycine.Name = "Glycine";
            glycine.Code = "G";
            glycine.Codons.Add(new Codon() { Triplet = "GGT" });
            glycine.Codons.Add(new Codon() { Triplet = "GGC" });
            glycine.Codons.Add(new Codon() { Triplet = "GGA" });
            glycine.Codons.Add(new Codon() { Triplet = "GGG" });
            commonDbContext.AminoAcids.AddIfNotExists(glycine, x => x.Code == glycine.Code);

            // 9. Histidine
            AminoAcid histidine = new AminoAcid();
            histidine.Name = "Histidine";
            histidine.Code = "H";
            histidine.Codons.Add(new Codon() { Triplet = "CAT" });
            histidine.Codons.Add(new Codon() { Triplet = "CAC" });
            commonDbContext.AminoAcids.AddIfNotExists(histidine, x => x.Code == histidine.Code);

            // 10. Isoleucine
            AminoAcid isoleucine = new AminoAcid();
            isoleucine.Name = "Isoleucine";
            isoleucine.Code = "I";
            isoleucine.Codons.Add(new Codon() { Triplet = "ATT" });
            isoleucine.Codons.Add(new Codon() { Triplet = "ATC" });
            isoleucine.Codons.Add(new Codon() { Triplet = "ATA" });
            commonDbContext.AminoAcids.AddIfNotExists(isoleucine, x => x.Code == isoleucine.Code);

            // 11. Leucine
            AminoAcid leucine = new AminoAcid();
            leucine.Name = "Leucine";
            leucine.Code = "L";
            leucine.Codons.Add(new Codon() { Triplet = "TTA" });
            leucine.Codons.Add(new Codon() { Triplet = "TTG" });
            leucine.Codons.Add(new Codon() { Triplet = "CTT" });
            leucine.Codons.Add(new Codon() { Triplet = "CTC" });
            leucine.Codons.Add(new Codon() { Triplet = "CTA" });
            leucine.Codons.Add(new Codon() { Triplet = "CTG" });
            commonDbContext.AminoAcids.AddIfNotExists(leucine, x => x.Code == leucine.Code);

            // 12. Lysine
            AminoAcid lysine = new AminoAcid();
            lysine.Name = "Lysine";
            lysine.Code = "K";
            lysine.Codons.Add(new Codon() { Triplet = "AAA" });
            lysine.Codons.Add(new Codon() { Triplet = "AAG" });
            commonDbContext.AminoAcids.AddIfNotExists(lysine, x => x.Code == lysine.Code);

            // 13. Methionine
            AminoAcid methionine = new AminoAcid();
            methionine.Name = "Methionine";
            methionine.Code = "M";
            methionine.Codons.Add(new Codon() { Triplet = "ATG" });
            commonDbContext.AminoAcids.AddIfNotExists(methionine, x => x.Code == methionine.Code);

            // 14. Phenylalanine
            AminoAcid phenylalanine = new AminoAcid();
            phenylalanine.Name = "Phenylalanine";
            phenylalanine.Code = "F";
            phenylalanine.Codons.Add(new Codon() { Triplet = "TTT" });
            phenylalanine.Codons.Add(new Codon() { Triplet = "TTC" });
            commonDbContext.AminoAcids.AddIfNotExists(phenylalanine, x => x.Code == phenylalanine.Code);

            // 15. Proline
            AminoAcid proline = new AminoAcid();
            proline.Name = "Proline";
            proline.Code = "P";
            proline.Codons.Add(new Codon() { Triplet = "CCT" });
            proline.Codons.Add(new Codon() { Triplet = "CCC" });
            proline.Codons.Add(new Codon() { Triplet = "CCA" });
            proline.Codons.Add(new Codon() { Triplet = "CCG" });
            commonDbContext.AminoAcids.AddIfNotExists(proline, x => x.Code == proline.Code);

            // 16. Serine
            AminoAcid serine = new AminoAcid();
            serine.Name = "Serine";
            serine.Code = "S";
            serine.Codons.Add(new Codon() { Triplet = "TCT" });
            serine.Codons.Add(new Codon() { Triplet = "TCC" });
            serine.Codons.Add(new Codon() { Triplet = "TCA" });
            serine.Codons.Add(new Codon() { Triplet = "TCG" });
            serine.Codons.Add(new Codon() { Triplet = "AGT" });
            serine.Codons.Add(new Codon() { Triplet = "AGC" });
            commonDbContext.AminoAcids.AddIfNotExists(serine, x => x.Code == serine.Code);

            // 17. Threonine
            AminoAcid threonine = new AminoAcid();
            threonine.Name = "Threonine";
            threonine.Code = "T";
            threonine.Codons.Add(new Codon() { Triplet = "ACT" });
            threonine.Codons.Add(new Codon() { Triplet = "ACC" });
            threonine.Codons.Add(new Codon() { Triplet = "ACA" });
            threonine.Codons.Add(new Codon() { Triplet = "ACG" });
            commonDbContext.AminoAcids.AddIfNotExists(threonine, x => x.Code == threonine.Code);

            // 18. Tryptophan
            AminoAcid tryptophan = new AminoAcid();
            tryptophan.Name = "Tryptophan";
            tryptophan.Code = "W";
            tryptophan.Codons.Add(new Codon() { Triplet = "TGG" });
            commonDbContext.AminoAcids.AddIfNotExists(tryptophan, x => x.Code == tryptophan.Code);

            // 19. Tyrosine
            AminoAcid tyrosine = new AminoAcid();
            tyrosine.Name = "Tyrosine";
            tyrosine.Code = "Y";
            tyrosine.Codons.Add(new Codon() { Triplet = "TAT" });
            tyrosine.Codons.Add(new Codon() { Triplet = "TAC" });
            commonDbContext.AminoAcids.AddIfNotExists(tyrosine, x => x.Code == tyrosine.Code);

            // 20. Valine
            AminoAcid valine = new AminoAcid();
            valine.Name = "Valine";
            valine.Code = "V";
            valine.Codons.Add(new Codon() { Triplet = "GTT" });
            valine.Codons.Add(new Codon() { Triplet = "GTC" });
            valine.Codons.Add(new Codon() { Triplet = "GTA" });
            valine.Codons.Add(new Codon() { Triplet = "GTG" });
            commonDbContext.AminoAcids.AddIfNotExists(valine, x => x.Code == valine.Code);

            //Stop codons
            Codon taa = new Codon() { Triplet = "TAA" };
            commonDbContext.Codons.AddIfNotExists(taa, x => x.Triplet == taa.Triplet);

            Codon tag = new Codon() { Triplet = "TAG" };
            commonDbContext.Codons.AddIfNotExists(tag, x => x.Triplet == tag.Triplet);

            Codon tga = new Codon() { Triplet = "TGA" };
            commonDbContext.Codons.AddIfNotExists(tga, x => x.Triplet == tga.Triplet);
        }

        private static void SeedRoles()
        {
            AddOrUpdateRole("Guest", "User does not have access to equipment");
            AddOrUpdateRole("Admin", "User has full access to the system");
        }

        private static void AddOrUpdateRole(string name, string description)
        {
            Role exisingRole = commonDbContext.Roles.SingleOrDefault(x => x.Name == name);
            if (exisingRole != null)
            {
                exisingRole.Description = description;
            }
            else
            {
                var newRole = new Role()
                {
                    Name = name,
                    Description = description
                };
                commonDbContext.Roles.Add(newRole);
            }
        }
    }
}