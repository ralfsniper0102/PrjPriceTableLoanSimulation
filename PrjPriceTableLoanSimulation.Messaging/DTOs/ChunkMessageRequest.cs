﻿using PrjPriceTableLoanSimulation.UseCase.Enums;

namespace PrjPriceTableLoanSimulation.Messaging.DTOs
{
    public class ChunkMessageRequest
    {
        public byte[] Payload { get; set; }
        public int TotalChunks { get; set; }
        public int CurrentChunk { get; set; }
        public RequestTypeEnum RequestType { get; set; }
        public int StatusCode { get; set; }
    }
}
