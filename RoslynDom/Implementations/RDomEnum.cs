﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using RoslynDom.Common;

namespace RoslynDom
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming",
       "CA1711:IdentifiersShouldNotHaveIncorrectSuffix",
       Justification = "Because this represents an enum, it's an appropriate name")]
    public class RDomEnum : RDomBase<IEnum, ISymbol>, IEnum
    {
        private AttributeCollection _attributes = new AttributeCollection();
        private RDomCollection<IEnumMember> _values;

        public RDomEnum(SyntaxNode rawItem, IDom parent, SemanticModel model)
            : base(rawItem, parent, model)
        { Initialize(); }

        internal RDomEnum(RDomEnum oldRDom)
            : base(oldRDom)
        {
            Initialize();
            Attributes.AddOrMoveAttributeRange(oldRDom.Attributes.Select(x => x.Copy()));
            var newValues = RoslynDomUtilities.CopyMembers(oldRDom._values);
            Members.AddOrMoveRange(newValues);
            AccessModifier = oldRDom.AccessModifier;
            DeclaredAccessModifier = oldRDom.DeclaredAccessModifier;
            UnderlyingType = oldRDom.UnderlyingType;
        }

        protected void Initialize()
        {
            _values = new RDomCollection<IEnumMember>(this);
        }

        public override IEnumerable<IDom> Children
        {
            get
            {
                var list = base.Children.ToList();
                list.AddRange(_values);
                return list;
            }
        }

        public override IEnumerable<IDom> Descendants
        {
            get
            {
                var list = base.Descendants.ToList();
                 list.AddRange(Children); 
                return list;
            }
        }

        public AttributeCollection Attributes
        { get { return _attributes; } }

        public string Name { get; set; }

        public string OuterName
        { get { return RoslynUtilities.GetOuterName(this); } }

        public string QualifiedName
        { get { return RoslynUtilities.GetQualifiedName(this); } }

        public string Namespace
        { get { return RoslynDomUtilities.GetNamespace(this.Parent); } }

        public bool IsNested
        { get { return (Parent is IType); } }
        public IType ContainingType
        { get { return Parent as IType; } }

        public RDomCollection<IEnumMember> Members
        { get { return _values; } }

        public AccessModifier AccessModifier { get; set; }
        public AccessModifier DeclaredAccessModifier { get; set; }

        public IReferencedType UnderlyingType { get; set; }

        public MemberKind MemberKind
        { get { return MemberKind.Enum; } }

        public StemMemberKind StemMemberKind
        { get { return StemMemberKind.Enum; } }

        public IStructuredDocumentation StructuredDocumentation { get; set; }

        public string Description { get; set; }
    }
}
