﻿using Microsoft.EntityFrameworkCore;
using ParkShark.Data;
using ParkShark.Domain.Members;
using ParkShark.Domain.ParkingLots;
using ParkShark.Services.Members;
using ParkShark.Services.ParkingLots;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ParkShark.Services.Tests.ParkingLotServiceTests
{
    public class ParkingLotServiceTests
    {
        private static DbContextOptions CreateNewInMemoryDatabase()
        {
            return new DbContextOptionsBuilder<ParkSharkDbContext>()
                .UseInMemoryDatabase("DivisionDb" + Guid.NewGuid().ToString("N"))
                .Options;
        }

        [Fact]
        public void GivenAddParkingLotToDBContext_WhenAddParkingLotToDbContext_ThenParkingLotIsAdded()
        {
            using (var context = new ParkSharkDbContext(CreateNewInMemoryDatabase()))
            {
                var city = City.CreateCity(2050, "Antwerpen", "Belgium");

                var parkingLot = ParkingLotBuilder.CreateNewParkingLot()
                    .WithName("test")
                    .WithAddress(Address.CreateAddress("Parkinglotstraat", "20a", city))
                    .WithContactPerson(ContactPerson.CreateNewContactPerson("Bas", "Adriaans", Address.CreateAddress("Contactpersoonstraat", "30", city), "bas@parking.com", "000000", ""))
                    .WithCapacity(20)
                    .WithDivision(Guid.NewGuid())
                    .WithPricePerHour(4.5m)
                    .Build();

                var service = new ParkingLotService(context);
                var result = service.CreateParkingLot(parkingLot);

                Assert.IsType<ParkingLot>(result);
            }
        }

        [Fact]
        public void GivenHappyPath2_WhenAddingNewParkingLotToDb_ObjectIsAddedToDb()
        {
            using (var context = new ParkSharkDbContext(CreateNewInMemoryDatabase()))
            {
                var city = City.CreateCity(2050, "Antwerpen", "Belgium");

                var parkingLot = ParkingLotBuilder.CreateNewParkingLot()
                    .WithName("test")
                    .WithAddress(Address.CreateAddress("Parkinglotstraat", "20a", city))
                    .WithContactPerson(ContactPerson.CreateNewContactPerson("Bas", "Adriaans", Address.CreateAddress("Contactpersoonstraat", "30", city), "bas@parking.com", "000000", ""))
                    .WithCapacity(20)
                    .WithDivision(Guid.NewGuid())
                    .WithPricePerHour(4.5m)
                    .Build();

                var service = new ParkingLotService(context);
                var result = service.CreateParkingLot(parkingLot);

                Assert.Single(context.ParkingLots);
            }
        }


        [Fact]
        public void GivenGetAllParkingLots_WhenRequestingAllParkingLots_ThenReturnListOfAllParkingLots()
        {
            using (var context = new ParkSharkDbContext(CreateNewInMemoryDatabase()))
            {
                var city = City.CreateCity(2050, "Antwerpen", "Belgium");

                var parkingLot1 = ParkingLotBuilder.CreateNewParkingLot()
                  .WithName("test")
                  .WithAddress(Address.CreateAddress("Parkinglotstraat", "20a", city))
                  .WithContactPerson(ContactPerson.CreateNewContactPerson("Bas", "Adriaans", Address.CreateAddress("Contactpersoonstraat", "30", city), "bas@parking.com", "000000", ""))
                  .WithCapacity(20)
                  .WithDivision(Guid.NewGuid())
                  .WithPricePerHour(4.5m)
                  .Build();
                var parkingLot2 = ParkingLotBuilder.CreateNewParkingLot()
                  .WithName("test2")
                  .WithAddress(Address.CreateAddress("Parkinglotstraat", "20a", city))
                  .WithContactPerson(ContactPerson.CreateNewContactPerson("Bas", "Adriaans", Address.CreateAddress("Contactpersoonstraat", "30", city), "bas@parking.com", "000000", ""))
                  .WithCapacity(20)
                  .WithDivision(Guid.NewGuid())
                  .WithPricePerHour(4.5m)
                  .Build();
                context.Set<ParkingLot>().Add(parkingLot1);
                context.Set<ParkingLot>().Add(parkingLot2);
                context.SaveChanges();

                var service = new ParkingLotService(context);
                var result = context.ParkingLots.CountAsync();
                Assert.Equal(2, result.Result);

            }
        }

        [Fact]
        public void GivenGetSingleParkingLot_WhenRequestingSingleParkingLot_ReturnRequestedParkingLot()
        {
            using (var context = new ParkSharkDbContext(CreateNewInMemoryDatabase()))
            { 
                var city = City.CreateCity(2050, "Antwerpen", "Belgium");
                var service = new ParkingLotService(context);
                var parkingLot = ParkingLotBuilder.CreateNewParkingLot()
                    .WithName("test")
                    .WithAddress(Address.CreateAddress("Parkinglotstraat", "20a", city))
                    .WithContactPerson(ContactPerson.CreateNewContactPerson("Bas", "Adriaans", Address.CreateAddress("Contactpersoonstraat", "30", city), "bas@parking.com", "000000", ""))
                    .WithCapacity(20)
                    .WithDivision(Guid.NewGuid())
                    .WithPricePerHour(4.5m)
                    .Build();
                context.Set<ParkingLot>().Add(parkingLot);
                var id = parkingLot.ParkingLotID;
                context.SaveChanges();

                var result = service.GetSingleParkingLot(id);

                Assert.IsType<ParkingLot>(result);
                Assert.Equal(id, result.ParkingLotID);
                Assert.Equal("test", result.Name);

            }
        }

        [Fact]
        public void GivenGetSingleParkingLotUnHappyPath_WhenRequestingSingleParkingLot_ReturnNull()
        {
            using (var context = new ParkSharkDbContext(CreateNewInMemoryDatabase()))
            {
                var service = new ParkingLotService(context);

                var id = new Guid();

                var result = service.GetSingleParkingLot(id);

                Assert.Null(result);
            }
        }
    }
}
