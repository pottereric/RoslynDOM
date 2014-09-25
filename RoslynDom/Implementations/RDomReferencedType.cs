﻿using System;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using RoslynDom.Common;
using System.Linq;
using System.Collections.Generic;

namespace RoslynDom
{

   public class RDomReferencedType : RDomBase<IReferencedType, ISymbol>, IReferencedType
   {
      // ITypeParameter not used because these are arguments, not declarations
      private RDomCollection<IReferencedType> _typeArguments;

      public RDomReferencedType(SyntaxNode rawItem, IDom parent, SemanticModel model)
            : base(rawItem, parent, model)
      { Initialize(); }

      internal RDomReferencedType(RDomReferencedType oldRDom)
          : base(oldRDom)
      {
         Initialize();
         Name = oldRDom.Name;
         Namespace = oldRDom.Namespace;
         DisplayAlias = oldRDom.DisplayAlias;
         IsArray = oldRDom.IsArray;
         ContainingType = oldRDom.ContainingType;
         TypeArguments.AddOrMoveRange(RoslynDomUtilities.CopyMembers(oldRDom._typeArguments));
      }

      private void Initialize()
      {
         _typeArguments = new RDomCollection<IReferencedType>(this);
      }

      public string Name { get; set; }
      public bool DisplayAlias { get; set; }
      public bool IsArray { get; set; }

      public string QualifiedName
      {
         get
         {
            var containingTypename = (ContainingType == null)
                                        ? ""
                                        : ContainingType.Name + ".";
            var ns = (string.IsNullOrEmpty(Namespace))
                        ? ""
                        : Namespace + ".";
            return ns + containingTypename + Name;
         }
      }

      public string Namespace { get; set; }

      public RDomCollection<IReferencedType> TypeArguments
      { get { return _typeArguments; } }

      public INamedTypeSymbol ContainingType { get; set; }
   }
}
