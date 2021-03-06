﻿using System;
using System.Data;
using SqlRepo.Abstractions;

namespace SqlRepo.ValueMemberSetters
{
    public class ByteValueMemberSetter : ValueMemberSetterBase
    {
        public ByteValueMemberSetter()
            : base(new[] {typeof(byte), typeof(byte?)}) { }

        protected override object GetValueByColumnIndex(IDataRecord dataRecord, int columnIndex)
        {
            if(dataRecord.IsDBNull(columnIndex))
            {
                return null;
            }

            return dataRecord.GetByte(columnIndex);
        }
    }
}