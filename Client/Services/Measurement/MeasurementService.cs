using YGate.Entities.BasedModel;
using YGate.Entities;
using System;

namespace YGate.Client.Services.Measurement
{
    public class MeasurementService : IMeasurementService   
    {
        HttpClientService httpClientService;
        public MeasurementService(HttpClientService httpClientService)
        {
            this.httpClientService = httpClientService;
        }
        public async Task<RequestResult> AddMeasurementCategory(MeasurementCategory measurementCategory)
        {
            RequestResult result = await httpClientService.GetPostAsync<RequestResult>(measurementCategory, "api/Measurement/AddCategory");
            return result;
        }

        public async Task<RequestResult> GetAllMeasurementCategory() {
            RequestResult result = await httpClientService.GetPostAsync<RequestResult>(null, "api/Measurement/GetAllCategory");
            return result;
        }

        public async Task<RequestResult> DeleteMeasurermentCategory(string guid) {
            RequestResult result = await httpClientService.GetPostAsync<RequestResult>(guid, "api/Measurement/DeleteCategory");
            return result;
        }

        public async Task<RequestResult> GetAllMeasurementUnit() {
            RequestResult result = await httpClientService.GetPostAsync<RequestResult>(null, "api/Measurement/GetAllUnit");
            return result;
        }

        public async Task<RequestResult> DeleteMeasurermentUnit(string guid) {
            RequestResult result = await httpClientService.GetPostAsync<RequestResult>(guid, "api/Measurement/DeleteUnit");
            return result;
        }

        public async Task<RequestResult> AddMeasurementUnit(MeasurementUnit measurementUnit) {
            RequestResult result = await httpClientService.GetPostAsync<RequestResult>(measurementUnit, "api/Measurement/AddUnit");
            return result;
        }
    }
}
