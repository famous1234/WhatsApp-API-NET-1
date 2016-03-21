﻿/**
 * Copyright (C) 2015 smndtrl
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using Google.ProtocolBuffers;
using System;
using Tr.Com.Eimza.LibAxolotl.Ecc;
using static Tr.Com.Eimza.LibAxolotl.State.StorageProtos;

namespace Tr.Com.Eimza.LibAxolotl.State
{
    public class SignedPreKeyRecord
    {
        private SignedPreKeyRecordStructure structure;

        public SignedPreKeyRecord(uint id, ulong timestamp, ECKeyPair keyPair, byte[] signature)
        {
            this.structure = SignedPreKeyRecordStructure.CreateBuilder()
                                                        .SetId(id)
                                                        .SetPublicKey(ByteString.CopyFrom(keyPair.GetPublicKey().Serialize()))
                                                        .SetPrivateKey(ByteString.CopyFrom(keyPair.GetPrivateKey().Serialize()))
                                                        .SetSignature(ByteString.CopyFrom(signature))
                                                        .SetTimestamp(timestamp)
                                                        .Build();
        }

        public SignedPreKeyRecord(byte[] serialized)
        {
            this.structure = SignedPreKeyRecordStructure.ParseFrom(serialized);
        }

        public uint GetId()
        {
            return this.structure.Id;
        }

        public ulong GetTimestamp()
        {
            return this.structure.Timestamp;
        }

        public ECKeyPair GetKeyPair()
        {
            try
            {
                ECPublicKey publicKey = Curve.DecodePoint(this.structure.PublicKey.ToByteArray(), 0);
                ECPrivateKey privateKey = Curve.DecodePrivatePoint(this.structure.PrivateKey.ToByteArray());

                return new ECKeyPair(publicKey, privateKey);
            }
            catch (InvalidKeyException e)
            {
                throw new Exception(e.Message);
            }
        }

        public byte[] GetSignature()
        {
            return this.structure.Signature.ToByteArray();
        }

        public byte[] Serialize()
        {
            return this.structure.ToByteArray();
        }
    }
}