using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace RestaurantAPI.Authorization
{
    public class MinimumAgeRequirementHandler : AuthorizationHandler<MinimumAgeRequirement>
    {
        private readonly ILogger<MinimumAgeRequirementHandler> _logger;

        public MinimumAgeRequirementHandler(ILogger<MinimumAgeRequirementHandler> logger) {
            _logger = logger;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumAgeRequirement requirement)
        {
            var dateOfBirth = DateTime.Parse(context.User.FindFirst(c => c.Type == "DateOfBirth")?.Value);

            var userEmail = context.User.FindFirst(c => c.Type == ClaimTypes.Name)?.Value;

            _logger.LogInformation("User {UserEmail} with date of birth {DateOfBirth}", userEmail, dateOfBirth);

            if (dateOfBirth.AddYears(requirement.MinimumAge) <= DateTime.Today)
            {
                _logger.LogInformation("User {UserEmail} meets the minimum age requirement of {MinimumAge}", userEmail, requirement.MinimumAge);
                context.Succeed(requirement);
            }
            else 
            {
                _logger.LogInformation("User {UserEmail} does not meet the minimum age requirement of {MinimumAge}", userEmail, requirement.MinimumAge);
            }
            return Task.CompletedTask;
        }
    }
}
