using YGate.Entities;
using YGate.Entities.BasedModel;

namespace YGate.Client.Services.Measurement
{
    public interface IMeasurementService
    {
        Task<RequestResult> AddMeasurementCategory(MeasurementCategory measurementCategory);

        Task<RequestResult> GetAllMeasurementCategory();

        Task<RequestResult> DeleteMeasurermentCategory(string guid);

        Task<RequestResult> GetAllMeasurementUnit();

        Task<RequestResult> DeleteMeasurermentUnit(string guid);

        Task<RequestResult> AddMeasurementUnit(MeasurementUnit measurementUnit);
    }
}
