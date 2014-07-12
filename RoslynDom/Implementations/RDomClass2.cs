﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RoslynDom.Common;

namespace RoslynDom
{
    public class RDomClassTypeFactory
               : RDomStemMemberFactory<RDomClass, ClassDeclarationSyntax>
    {
        public RDomClassTypeFactory(RDomFactoryHelper helper)
            : base(helper)
        { }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// I'm not currently supporting parameters (am supporting type parameters) because I don't understand them
    /// </remarks>
    public class RDomClass2 : RDomBaseType<IClass, ClassDeclarationSyntax>, IClass
    {
        internal RDomClass2(ClassDeclarationSyntax rawItem,
            IEnumerable<ITypeMember> members,
            params PublicAnnotation[] publicAnnotations)
            : base(rawItem, MemberKind.Class, StemMemberKind.Class, members, publicAnnotations)
        {
            Initialize();
        }

        internal RDomClass2(RDomClass oldRDom)
             : base(oldRDom)
        {
            BaseType = oldRDom.BaseType.Copy();
            IsAbstract = oldRDom.IsAbstract;
            IsSealed = oldRDom.IsSealed;
            IsStatic = oldRDom.IsStatic;
        }

        protected override void Initialize()
        {
            base.Initialize();
            BaseType = new RDomReferencedType(TypedSymbol.DeclaringSyntaxReferences, TypedSymbol.BaseType);
            IsAbstract = Symbol.IsAbstract;
            IsSealed = Symbol.IsSealed;
            IsStatic = Symbol.IsStatic;
        }

        public override ClassDeclarationSyntax BuildSyntax()
        {
            var modifiers = BuildModfierSyntax();
            var node = SyntaxFactory.ClassDeclaration(Name)
                            .WithModifiers(modifiers);

            node = RoslynUtilities.UpdateNodeIfListNotEmpty(BuildMembers(true), node, (n, l) => n.WithMembers(l));
            //node = RoslynUtilities.UpdateNodeIfListNotEmpty(BuildTypeParameterList(), node, (n, l) => n.WithTypeParameters(l));
            //node = RoslynUtilities.UpdateNodeIfListNotEmpty(BuildConstraintClauses(), node, (n, l) => n.WithTypeConstraints(l));
            node = RoslynUtilities.UpdateNodeIfListNotEmpty(BuildAttributeListSyntax(), node, (n, l) => n.WithAttributeLists(l));

            return (ClassDeclarationSyntax)RoslynUtilities.Format(node);
        }

   
        public IEnumerable<IClass> Classes
        { get { return Members.OfType<IClass>(); } }

        public IEnumerable<IType> Types
        //{ get { return Classes.Concat<IStemMember>(Structures).Concat(Interfaces).Concat(Enums); } }
        { get { return Members.OfType<IType>(); } }


        public IEnumerable<IStructure> Structures
        {
            get
            { return Members.OfType<IStructure>(); }
        }

        public IEnumerable<IInterface> Interfaces
        {
            get
            { return Members.OfType<IInterface>(); }
        }

        public IEnumerable<IEnum> Enums
        {
            get
            { return Members.OfType<IEnum>(); }
        }

        public bool IsAbstract { get; set; }

        public bool IsSealed { get; set; }

        public bool IsStatic { get; set; }


        public IReferencedType BaseType { get; set; }

        public IEnumerable<IReferencedType> ImplementedInterfaces
        {
            get
            {
                return this.ImpementedInterfacesFrom(false);
            }
        }

        public IEnumerable<IReferencedType> AllImplementedInterfaces
        { get { return this.ImpementedInterfacesFrom(true); } }

        public string ClassName { get { return this.Name; } }
    }
}