﻿namespace AutoGene.Presentation.Host.Models
{
    public class Navbar
    {
        public int Id { get; set; }
        public string nameOption { get; set; }
        public string controller { get; set; }
        public string action { get; set; }
        public string area { get; set; }
        public string imageClass { get; set; }
        public string activeli { get; set; }
        public bool status { get; set; }
        public int parentId { get; set; }
        public bool isParent { get; set; }
    }
}