using LearnixCRM.Application.DTOs.AdminReports;
using LearnixCRM.Application.DTOs.SalesAnalytics;
using LearnixCRM.Application.Interfaces.Repositories;
using LearnixCRM.Application.Interfaces.Services;

namespace LearnixCRM.Application.Services
{
    public class SalesAnalyticsService : ISalesAnalyticsService
    {
        private readonly ISalesAnalyticsRepository _repository;

        public SalesAnalyticsService(ISalesAnalyticsRepository repository)
        {
            _repository = repository;
        }

        public async Task<SalesAnalyticsSummaryDto> GetSalesSummaryAsync()
        {
            try
            {
                var result = await _repository.GetSalesSummaryAsync();

                if (result == null)
                    throw new InvalidOperationException("Sales summary data not found.");

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while fetching sales summary: {ex.Message}");
            }
        }

        public async Task<IEnumerable<MonthlySalesDto>> GetMonthlySalesAsync()
        {
            try
            {
                var result = await _repository.GetMonthlySalesAsync();

                if (result == null || !result.Any())
                    throw new InvalidOperationException("Monthly sales data not available.");

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while fetching monthly sales data: {ex.Message}");
            }
        }

        public async Task<IEnumerable<ManagerPerformanceDto>> GetManagerPerformanceAsync()
        {
            try
            {
                var result = await _repository.GetManagerPerformanceAsync();

                if (result == null || !result.Any())
                    throw new InvalidOperationException("Manager performance data not found.");

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while fetching manager performance: {ex.Message}");
            }
        }
    }
}