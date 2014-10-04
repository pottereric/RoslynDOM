﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using RoslynDom.Common;

namespace RoslynDom
{
   public class RDomUsingStatement : RDomBase<IUsingStatement, ISymbol>, IUsingStatement
   {
      private RDomCollection<IStatementCommentWhite> _statements;

      public RDomUsingStatement(SyntaxNode rawItem, IDom parent, SemanticModel model)
         : base(rawItem, parent, model)
      { Initialize(); }

      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
         "CA1811:AvoidUncalledPrivateCode", Justification = "Called via Reflection")]
      internal RDomUsingStatement(RDomUsingStatement oldRDom)
          : base(oldRDom)
      {
         Initialize();
         var statements = RoslynDomUtilities.CopyMembers(oldRDom.Statements);
         StatementsAll.AddOrMoveRange(statements);
         HasBlock = oldRDom.HasBlock;
         if (oldRDom.Expression != null) Expression = oldRDom.Expression.Copy();
         if (oldRDom.Variable != null) Variable = oldRDom.Variable.Copy();
      }

      private void Initialize()
      {
         _statements = new RDomCollection<IStatementCommentWhite>(this);
      }

      public override IEnumerable<IDom> Children
      {
         get
         {
            var list = base.Children.ToList();
            list.AddRange(Statements);
            return list;
         }
      }

      public IExpression Expression { get; set; }
      public IVariableDeclaration Variable { get; set; }
      public bool HasBlock { get; set; }

      public IEnumerable<IStatement> Statements
      { get { return _statements.OfType<IStatement>().ToList(); } }

      public RDomCollection<IStatementCommentWhite> StatementsAll
      { get { return _statements; } }

   }
}
