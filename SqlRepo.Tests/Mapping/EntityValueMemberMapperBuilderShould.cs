﻿using System;
using System.Reflection;
using FluentAssertions;
using NUnit.Framework;
using SqlRepo.Abstractions;
using SqlRepo.Tests.TestObjects;

namespace SqlRepo.Tests
{
    [TestFixture]
    public class EntityValueMemberMapperBuilderShould
    {
        private const string ColumnName = "Id";

        [SetUp]
        public void SetUp()
        {
            this.memberInfo = typeof(Person).GetMember(ColumnName)[0];
            this.target = new EntityValueMemberMapperBuilder(this.memberInfo);
        }

        [Test]
        public void BuildCorrectType()
        {
            var actual = this.target.Build();

            actual.Should()
                  .NotBeNull("Actual was null");
            actual.Should()
                  .BeOfType<EntityValueMemberMapper>("Actual was not of correct type");
        }

        [Test]
        public void SetMemberInfoCorrectly()
        {
            var actual = this.target.Build();

            actual.MemberInfo.Should()
                  .Be(this.memberInfo);
        }

        [Test]
        public void SetUpColumnNameStrategyCorrectly()
        {
            this.target.MapFromColumnName(ColumnName);

            var actual = this.target.Build();

            actual
                .MappingStrategy.Should()
                .Be(EntityValueMemberMappingStrategy.ColumnName, "Actual strategy was not set as expected");

            actual.ColumnName.Should()
                  .Be(ColumnName, "Actual column name was not set correctly");
        }

        [Test]
        public void SetColumnIndexStrategyCorrectly()
        {
            this.target.MapFromIndex(0);

            var actual = this.target.Build();

            actual
                .MappingStrategy.Should()
                .Be(EntityValueMemberMappingStrategy.ColumnIndex, "Actual strategy was not set as expected");

            actual.ColumnIndex.Should()
                  .Be(0, "Actual column index was not set as expected");

        }

        private IEntityValueMemberMapperBuilder target;
        private MemberInfo memberInfo;
    }
}