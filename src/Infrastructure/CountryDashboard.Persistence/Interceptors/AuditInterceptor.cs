
namespace CountryDashboard.Persistence.Interceptors
{
    public class AuditInterceptor(/*ITokenInfo tokenInfo*/) : SaveChangesInterceptor
    {
        //private readonly ITokenInfo _tokenInfo = tokenInfo;

        public override InterceptionResult<int> SavingChanges(
            DbContextEventData eventData,
            InterceptionResult<int> result)
        {
            ApplyAudit(eventData.Context);
            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            ApplyAudit(eventData.Context);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void ApplyAudit(DbContext? context)
        {
            if (context == null) return;

            var now = DateTime.UtcNow;
            var currentUser = 0; /*_tokenInfo.UserId;*/

            foreach (var entry in context.ChangeTracker.Entries())
            {
                if (entry.Entity == null) continue;

                // Audit - Created
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.GetType().GetProperty("CreatedDate")?.SetValue(entry.Entity, now);
                    entry.Entity.GetType().GetProperty("CreatedBy")?.SetValue(entry.Entity, currentUser);
                }

                // Audit - Modified (including soft delete)
                if (entry.State == EntityState.Modified)
                {
                    entry.Entity.GetType().GetProperty("ModifiedDate")?.SetValue(entry.Entity, now);
                    entry.Entity.GetType().GetProperty("ModifiedBy")?.SetValue(entry.Entity, currentUser);
                }
            }
        }
    }
}
