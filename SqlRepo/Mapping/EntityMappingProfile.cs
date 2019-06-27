﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using SqlRepo.Abstractions;

namespace SqlRepo
{
    public class EntityMappingProfile<T> : IEntityMappingProfile<T>
        where T: class, new()
    {
        private readonly IDictionary<MemberInfo, IEntityValueMemberMapper> memberMappers;
        private readonly IDictionary<MemberInfo, IEntityMappingProfile> memberMappingProfiles;

        public EntityMappingProfile()
        {
            this.TargetType = typeof(T);
            this.memberMappers = new Dictionary<MemberInfo, IEntityValueMemberMapper>();
            this.memberMappingProfiles = new Dictionary<MemberInfo, IEntityMappingProfile>();
        }

        public Type TargetType { get; }

        public IEntityMappingProfile<T> ForEnumerableMember<TEnumerable, TItem>(Expression<Func<T, IEnumerable<TItem>>> selector,
            IEntityMappingProfile mappingProfile)
            where TEnumerable: class, IEnumerable<TItem>, new() where TItem: class, new()
        {
            return null;
        }

        public IEntityMappingProfile<T> ForMember<TMember>(Expression<Func<T, TMember>> selector,
            Action<IEntityValueMemberMapperBuilderConfig> config)
        {
            var memberInfo = selector.GetMemberExpression()
                                     .Member;

            var entityMemberMapperBuilder = new EntityValueMemberMapperBuilder(memberInfo);
            config(entityMemberMapperBuilder);

            this.memberMappers.Add(memberInfo, entityMemberMapperBuilder.Build());

            return this;
        }

        public IEntityMappingProfile<T> ForMember<TMember>(Expression<Func<T, TMember>> selector,
            IEntityMappingProfile mappingProfile)
            where TMember: class, new()
        {
            var memberInfo = selector.GetMemberExpression()
                                     .Member;

            this.memberMappingProfiles.Add(memberInfo, mappingProfile);

            return this;
        }

        public IEntityMappingProfile<T> ForMember<TMember>(Expression<Func<T, TMember>> selector,
            Action<IEntityMappingProfile<TMember>> config)
            where TMember: class, new()
        {
            var entityMappingProfile = new EntityMappingProfile<TMember>();
            config(entityMappingProfile);

            return this.ForMember(selector, entityMappingProfile);
        }

        public void Map(object entity, IDataRecord dataRecord)
        {
            foreach(var mapper in this.memberMappers.Values)
            {
                mapper.Map(entity, dataRecord);
            }

            foreach(var memberMappingProfile in this.memberMappingProfiles)
            {
                var memberInfo = memberMappingProfile.Key;
                var profile = memberMappingProfile.Value;
                var memberInstance = profile.TargetType.CreateInstance();
                profile.Map(memberInstance, dataRecord);
                memberInfo.SetValue(entity, memberInstance);
            }
        }
    }
}