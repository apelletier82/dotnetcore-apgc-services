using System;

namespace Framework.Entities.Owned
{
    public class Audit
    {                
        public string User { get; private set; } = "";
        public DateTimeOffset? Date { get; private set; }
        
        public virtual void DoAudit(string auditUser)
        {            
            User = auditUser;
            Date = DateTimeOffset.Now;
        }
    }
}