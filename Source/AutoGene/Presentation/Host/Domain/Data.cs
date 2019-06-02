using System.Collections.Generic;
using System.Linq;
using AutoGene.Presentation.Host.Models;

namespace AutoGene.Presentation.Host.Domain
{
    public class Data
    {
        public IEnumerable<Navbar> navbarItems()
        {
            var menu = new List<Navbar>();
            menu.Add(new Navbar { Id = 1, nameOption = "System Monitor", controller = "SystemMonitor", action = "Index", imageClass = "fa fa-dashboard fa-fw", status = true, isParent = false, parentId = 0 });
            menu.Add(new Navbar { Id = 2, nameOption = "Gene Editor", controller = "GeneEditor", action = "Index", imageClass = "fa fa-sitemap fa-fw", status = true, isParent = false, parentId = 0 });
//            menu.Add(new Navbar { Id = 2, nameOption = "Монитор циклов", controller = "Home", action = "FlotCharts", status = true, isParent = false, parentId = 1 });
//            menu.Add(new Navbar { Id = 3, nameOption = "Монитор колонок", controller = "Home", action = "MorrisCharts", status = true, isParent = false, parentId = 1 });
            menu.Add(new Navbar { Id = 3, nameOption = "Cycle Editor", controller = "CycleEditor", action = "Index", imageClass = "fa fa-cogs fa-fw", status = true, isParent = false, parentId = 0 });
            menu.Add(new Navbar { Id = 4, nameOption = "System Configuration", controller = "SystemConfiguration", action = "Index", imageClass = "fa fa-cog fa-fw", status = true, isParent = false, parentId = 0 });
            menu.Add(new Navbar { Id = 5, nameOption = "Oligo Synthesizer", controller = "OligoSynthesizer", action = "Index", imageClass = "fa fa-flask fa-fw", status = true, isParent = false, parentId = 0 });
            menu.Add(new Navbar { Id = 6, nameOption = "Gene Synthesizer", controller = "GeneSynthesizer", action = "Index", imageClass = "fa fa-link fa-fw", status = true, isParent = false, parentId = 0 });
            menu.Add(new Navbar { Id = 7, nameOption = "Manual Control", controller = "ManualControl", action = "Index", imageClass = "fa fa-sliders fa-fw", status = true, isParent = false, parentId = 0 });
            //            menu.Add(new Navbar { Id = 5, nameOption = "Оптимизатор программы синтеза", controller = "Home", action = "Buttons", imageClass = "fa fa-bar-chart-o fa-fw", status = true, isParent = false, parentId = 0 });
            //            menu.Add(new Navbar { Id = 6, nameOption = "Редактор метода синтеза", controller = "Home", action = "Notifications", imageClass = "fa fa-files-o fa-fw", status = true, isParent = false, parentId = 0 });
            //            menu.Add(new Navbar { Id = 7, nameOption = "Диагностика узлов оборудования", controller = "Home", action = "Typography", imageClass = "fa fa-wrench fa-fw", status = true, isParent = false, parentId = 0 });

            //            menu.Add(new Navbar { Id = 1, nameOption = "Dashboard", controller = "Home", action = "Index", imageClass = "fa fa-dashboard fa-fw", status = true, isParent = false, parentId = 0 });
            //            menu.Add(new Navbar { Id = 2, nameOption = "Charts", imageClass = "fa fa-bar-chart-o fa-fw", status = true, isParent = true, parentId = 0 });
            //            menu.Add(new Navbar { Id = 3, nameOption = "Flot Charts", controller = "Home", action = "FlotCharts", status = true, isParent = false, parentId = 2 });
            //            menu.Add(new Navbar { Id = 4, nameOption = "Morris.js Charts", controller = "Home", action = "MorrisCharts", status = true, isParent = false, parentId = 2 });
            //            menu.Add(new Navbar { Id = 5, nameOption = "Tables", controller = "Home", action = "Tables", imageClass = "fa fa-table fa-fw", status = true, isParent = false, parentId = 0 });
            //            menu.Add(new Navbar { Id = 6, nameOption = "Forms", controller = "Home", action = "Forms", imageClass = "fa fa-edit fa-fw", status = true, isParent = false, parentId = 0 });
            //            menu.Add(new Navbar { Id = 7, nameOption = "UI Elements", imageClass = "fa fa-wrench fa-fw", status = true, isParent = true, parentId = 0 });
            //            menu.Add(new Navbar { Id = 8, nameOption = "Panels and Wells", controller = "Home", action = "Panels", status = true, isParent = false, parentId = 7 });
            //            menu.Add(new Navbar { Id = 9, nameOption = "Buttons", controller = "Home", action = "Buttons", status = true, isParent = false, parentId = 7 });
            //            menu.Add(new Navbar { Id = 10, nameOption = "Notifications", controller = "Home", action = "Notifications", status = true, isParent = false, parentId = 7 });
            //            menu.Add(new Navbar { Id = 11, nameOption = "Typography", controller = "Home", action = "Typography", status = true, isParent = false, parentId = 7 });
            //            menu.Add(new Navbar { Id = 12, nameOption = "Icons", controller = "Home", action = "Icons", status = true, isParent = false, parentId = 7 });
            //            menu.Add(new Navbar { Id = 13, nameOption = "Grid", controller = "Home", action = "Grid", status = true, isParent = false, parentId = 7 });
            //            menu.Add(new Navbar { Id = 14, nameOption = "Multi-Level Dropdown", imageClass = "fa fa-sitemap fa-fw", status = true, isParent = true, parentId = 0 });
            //            menu.Add(new Navbar { Id = 15, nameOption = "Second Level Item", status = true, isParent = false, parentId = 14 });
            //            menu.Add(new Navbar { Id = 16, nameOption = "Sample Pages", imageClass = "fa fa-files-o fa-fw", status = true, isParent = true, parentId = 0 });
            //            menu.Add(new Navbar { Id = 17, nameOption = "Blank Page", controller = "Home", action = "Blank", status = true, isParent = false, parentId = 16 });
            //            menu.Add(new Navbar { Id = 18, nameOption = "Login Page", controller = "Home", action = "Login", status = true, isParent = false, parentId = 16 });

            return menu.ToList();
        }
    }
}