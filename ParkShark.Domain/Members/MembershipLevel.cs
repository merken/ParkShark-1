﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ParkShark.Domain.Members
{
    public class MembershipLevel
    {
        public MembershipLevelEnum Membership { get; set; }
        public string Name { get; set; }
        public decimal MonthlyCost { get; set; }
        public float PSAPriceReductionPercentage { get; set; }
        public TimeSpan PSAMaxTimeInHours { get; set; }
        public List<Member> members { get; set; }

        public MembershipLevel()
        {
            this.members = new List<Member>();
        }
    }
}