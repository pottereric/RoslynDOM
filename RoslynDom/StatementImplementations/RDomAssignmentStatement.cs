﻿using Microsoft.CodeAnalysis;
using RoslynDom.Common;

namespace RoslynDom
{
    public class RDomAssignmentStatement : RDomBase<IAssignmentStatement, ISymbol>, IAssignmentStatement
    {

        public RDomAssignmentStatement(SyntaxNode rawItem, SemanticModel model)
           : base(rawItem, model)
        { }

        internal RDomAssignmentStatement(RDomAssignmentStatement oldRDom)
             : base(oldRDom)
        {
            VarName = oldRDom.VarName;
            Expression = oldRDom.Expression;
        }

        public string VarName { get; set; }
        public IExpression Expression { get; set; }
    }
}
