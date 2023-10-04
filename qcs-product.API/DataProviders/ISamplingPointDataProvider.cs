using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using qcs_product.API.Models;

namespace qcs_product.API.DataProviders
{
    public interface ISamplingPointDataProvider
    {
        public Task<SamplingPoint> Insert(SamplingPoint samplingPoint);
        public Task<SamplingPoint> Update(SamplingPoint samplingPoint);
        public Task<SamplingPoint> GetByCode(string code);
        public Task<SamplingPoint> GetByCodeAndRoomCode(string code, string roomCode);
        public Task<SamplingPoint> GetByCodeToolCode(string samplingPointCode, string toolCode);
        public Task<RelSamplingTestParam> InsertRelSamplingParam(RelSamplingTestParam relSamplingTestParam);
        // public Task<RelSamplingTool> GetRelToolSamplingByCodeAndSamplingPointCode(string samplingPointCode, string toolCode);
        // public Task<RelSamplingTool> InsertRelToolSampling(RelSamplingTool relSamplingTool);
        // public Task<RelSamplingTool> UpdateRelToolSampling(RelSamplingTool relSamplingTool);
    }
}
