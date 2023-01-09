using System;
using System.Collections.Generic;
using System.Text;

namespace ElasticClientTest.ElasticModels
{
    public static class IndexDeciderUtils
    {
        public const string s_defaultIndex = "logs-azure.platformlogs-default";

        public const string s_activityIndex = "logs-azure.activitylogs-default";

        public const string s_auditIndex = "logs-azure.auditlogs-default";

        public const string s_signinIndex = "logs-azure.signinlogs-default";

        public static readonly ISet<string> s_subscriptionLogCategories = new HashSet<string> { "Administrative", "Security", "ServiceHealth", "Alert", "Recommendation", "Policy", "Autoscale", "ResourceHealth" };

        public static readonly ISet<string> s_auditLogCategories = new HashSet<string> { "AuditLogs" };

        public static readonly ISet<string> s_signinLogCategories = new HashSet<string> { "ManagedIdentitySignInLogs", "NonInteractiveUserSignInLogs", "SignInLogs", "ServicePrincipalSignInLogs", "ADFSSignInLogs" };

        public static string GetIndexName(LogRecordMetadata metadata)
        {
            if (metadata == null || metadata.Category == null)
            {
                return s_defaultIndex;
            }

            if (s_subscriptionLogCategories.Contains(metadata.Category))
            {
                return s_activityIndex;
            }

            if (s_auditLogCategories.Contains(metadata.Category))
            {
                return s_auditIndex;
            }

            if (s_signinLogCategories.Contains(metadata.Category))
            {
                return s_signinIndex;
            }

            return s_defaultIndex;
        }
    }
}
