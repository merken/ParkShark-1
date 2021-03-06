﻿using Microsoft.EntityFrameworkCore;
using ParkShark.Data;
using ParkShark.Domain.Members;
using ParkShark.Services.Members;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace ParkShark.Services.Tests.MemberServicesTests
{
    public class MemberServiceTests
    {
        private static DbContextOptions CreateNewInMemoryDatabase()
        {
            return new DbContextOptionsBuilder<ParkSharkDbContext>()
                .UseInMemoryDatabase("DivisionDb" + Guid.NewGuid().ToString("N"))
                .Options;
        }

        [Fact]
        public void GivenAddMemberToDBContext_WhenAddMemberToDbContext_ThenMemberIsAdded()
        {
            using (var context = new ParkSharkDbContext(CreateNewInMemoryDatabase()))
            {
                context.Set<MembershipLevel>().Add(new MembershipLevel() { MemberShipLevelId = 0, Name = "Bronze", MonthlyCost = 0, PSAPriceReductionPercentage = 0, PSAMaxTimeInHours = new TimeSpan(4, 0, 0) });
                context.SaveChanges();

                var city = City.CreateCity(2050, "Antwerpen", "Belgium");

                var service = new MemberService(context);

                var member = new DummyMemberObject() { FirstName = "lars", LastName = "Peelman", Address = Address.CreateAddress("test", "5", city), MembershipLevel = MembershipLevelEnum.Bronze };

                var result = service.CreateNewMember(member);

                Assert.IsType<Member>(result);
            }
        }

        [Fact]
        public void GivenHappyPath2_WhenAddingNewMemberToDb_ObjectIsAddedToDb()
        {
            using (var context = new ParkSharkDbContext(CreateNewInMemoryDatabase()))
            {
                context.Set<MembershipLevel>().Add(new MembershipLevel() { MemberShipLevelId = 0, Name = "Bronze", MonthlyCost = 0, PSAPriceReductionPercentage = 0, PSAMaxTimeInHours = new TimeSpan(4, 0, 0) });
                context.SaveChanges();

                var city = City.CreateCity(2050, "Antwerpen", "Belgium");

                var member = new DummyMemberObject() { FirstName = "lars", LastName = "Peelman", Address = Address.CreateAddress("test", "5", city), MembershipLevel = MembershipLevelEnum.Bronze };

                var service = new MemberService(context);
                var result = service.CreateNewMember(member);

                Assert.Single(service.GetAllMembers());
            }
        }


        [Fact]
        public void GivenGetAllMembers_WhenRequestingAllMembers_ThenReturnListOfAllMembers()
        {
            using (var context = new ParkSharkDbContext(CreateNewInMemoryDatabase()))
            {
                var memberShipLevel = new MembershipLevel();

                var city = City.CreateCity(2050, "Antwerpen", "Belgium");

                context.Set<Member>().Add(Member.CreateMember("lars", "Peelman", Address.CreateAddress("test", "5", city), MembershipLevelEnum.Bronze, memberShipLevel));
                context.Set<Member>().Add(Member.CreateMember("laeeers", "ee", Address.CreateAddress("test", "5", city), MembershipLevelEnum.Bronze, memberShipLevel));
                context.SaveChanges();

                var service = new MemberService(context);
                var result = service.GetAllMembers().Count;

                Assert.Equal(2, result);
            }
        }

        [Fact]
        public void GivenGetSingleMember_WhenRequestingSingleMember_ReturnRequestedMember()
        {
            using (var context = new ParkSharkDbContext(CreateNewInMemoryDatabase()))
            {
                var service = new MemberService(context);
                var memberShipLevel = new MembershipLevel();
                var city = City.CreateCity(2050, "Antwerpen", "Belgium");

                var member = Member.CreateMember("lars", "Peelman", Address.CreateAddress("test", "5", city), MembershipLevelEnum.Gold, memberShipLevel);
                context.Set<Member>().Add(member);
                context.SaveChanges();

                var id = member.MemberId;

                var result = service.GetMember(id);

                Assert.IsType<Member>(result);
                Assert.Equal(id, result.MemberId);
                Assert.Equal("lars", result.FirstName);
                Assert.Equal("Peelman", result.LastName);
            }
        }

        [Fact]
        public void GivenGetSingleMemberUnHappyPath_WhenRequestingSingleMember_ReturnNull()
        {
            using (var context = new ParkSharkDbContext(CreateNewInMemoryDatabase()))
            {
                var service = new MemberService(context);
                var memberShipLevel = new MembershipLevel();
                var city = City.CreateCity(2050, "Antwerpen", "Belgium");
                var member = Member.CreateMember("lars", "Peelman", Address.CreateAddress("test", "5", city), MembershipLevelEnum.Gold, memberShipLevel);

                context.Set<Member>().Add(member);
                context.SaveChanges();

                var fakeID = Guid.NewGuid();

                var result = service.GetMember(fakeID);

                Assert.Null(result);
            }
        }
    }
}
