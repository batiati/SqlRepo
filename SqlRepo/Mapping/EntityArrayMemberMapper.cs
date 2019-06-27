﻿using System;
using System.Data;
using System.Reflection;
using SqlRepo.Abstractions;

namespace SqlRepo
{
    public class EntityArrayMemberMapper<TItem> : IEntityMemberMapper
        where TItem: class, new()
    {
        private readonly IEntityMappingProfile<TItem> itemMappingProfile;

        public EntityArrayMemberMapper(MemberInfo memberInfo, IEntityMappingProfile<TItem> itemMappingProfile)
        {
            this.itemMappingProfile = itemMappingProfile;
            this.MemberInfo = memberInfo;
        }

        public MemberInfo MemberInfo { get; }

        public void Map(object entity, IDataRecord dataRecord)
        {
            var itemInstance = (TItem)Activator.CreateInstance(typeof(TItem));
            this.itemMappingProfile.Map(itemInstance, dataRecord);

            if(!(this.MemberInfo.GetValue(entity) is TItem[] currentValue))
            {
                currentValue = new[] {itemInstance};
            }
            else
            {
                var newItemIndex = currentValue.Length;
                var newLength = newItemIndex + 1;
                Array.Resize(ref currentValue, newLength);
                currentValue[newItemIndex] = itemInstance;
            }

            this.MemberInfo.SetValue(entity, currentValue);
        }
    }
}