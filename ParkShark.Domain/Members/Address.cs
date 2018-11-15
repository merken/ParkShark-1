﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ParkShark.Domain.Members
{
    public class Address
    {
        public Guid AddressId { get; private set; }
        public string StreetName { get; private set; }
        public string StreetNumber { get; private set; }
        public int ZIP { get; private set; }

        private Address(string streetName, string streetNumber, int zip)
        {
            AddressId = new Guid();
            StreetName = streetName;
            StreetNumber = streetNumber;
            ZIP = zip;
        }

        public static Address CreateAddress(string streetName, string streetNumber, int zip)
        {
            if(streetName == null || streetNumber == null || zip.ToString().Length != 4)
            {
                return null;
            }

            return new Address(streetName, streetNumber, zip);
        }
    }
}