using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using BILib;

namespace SpecEditor
{
    public class SpecService:DbService
    {
        public SpecService(string constr):base(constr)
        { }

        public void CommitSpecification(BISpecification spec)
        {
            string updateValidFlagStatement = "UPDATE [BI_Specification] SET [Validation]=0 WHERE [Plan]='<Plan>' AND [Validation]=1\n".Replace("<Plan>",spec.Plan);
            string insertNewSpecStatement = "INSERT [BI_Specification] ([Plan],[Version],[Content],[Load_Time],[Validation]) VALUES ('<Plan>','<Version>','<Content>',GETDATE(),1)\n"
                .Replace("<Plan>",spec.Plan).Replace("<Version>",spec.Version).Replace("<Content>",BISpecification.Serialize(spec));
            string cmd = "set xact_abort on\nbegin tran\n<statement>\ncommit tran\ngo".Replace("<statement>",updateValidFlagStatement+insertNewSpecStatement);
            Execute(cmd);
        }

        public DataTable GetSpecificationList()
        {
            string cmd = "SELECT * FROM [BI_Specification]";
            return Query(cmd);
        }
    }
}
