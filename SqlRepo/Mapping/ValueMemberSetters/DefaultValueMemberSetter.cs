﻿using System;
using System.Data;
using System.Linq;
using SqlRepo.Abstractions;

namespace SqlRepo.ValueMemberSetters
{
    public class DefaultValueMemberSetter : ValueMemberSetterBase
    {
        public DefaultValueMemberSetter()
            : base(Enumerable.Empty<Type>()) { }

        public override bool CanSet(Type type)
        {
            return true;
        }

        protected override object GetValueByColumnIndex(IDataRecord dataRecord, int columnIndex)
        {
            if(dataRecord.IsDBNull(columnIndex))
            {
                return null;
            }

            return dataRecord.GetValue(columnIndex);
        }
    }
}