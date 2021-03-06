﻿using System;
using System.Collections.Generic;

namespace Data.EF.Models
{
    public partial class Market
    {
        public Market()
        {
            Flat = new HashSet<Flat>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Flat> Flat { get; set; }
    }
}
