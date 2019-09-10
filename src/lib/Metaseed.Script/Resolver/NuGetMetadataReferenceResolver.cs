﻿using System;
using System.Collections.Immutable;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Scripting;

namespace Metaseed.Metatool.Script.Resolver
{
    /// <summary>
    /// A <see cref="MetadataReferenceResolver"/> decorator that handles
    /// references to NuGet packages in scripts.  
    /// </summary>
    public class NuGetMetadataReferenceResolver : MetadataReferenceResolver
    {
        private readonly MetadataReferenceResolver _metadataReferenceResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="NuGetMetadataReferenceResolver"/> class.
        /// </summary>
        /// <param name="metadataReferenceResolver">The target <see cref="MetadataReferenceResolver"/>.</param>                
        public NuGetMetadataReferenceResolver(string workingDirectory,MetadataReferenceResolver metadataReferenceResolver=null)
        {
            _metadataReferenceResolver = metadataReferenceResolver;
            if(_metadataReferenceResolver == null)
                _metadataReferenceResolver = ScriptMetadataResolver.Default.WithBaseDirectory(workingDirectory);
        }
        
        public override bool Equals(object other)
        {
            return _metadataReferenceResolver.Equals(other);
        }

        public override int GetHashCode()
        {
            return _metadataReferenceResolver.GetHashCode();
        }

        public override bool ResolveMissingAssemblies => _metadataReferenceResolver.ResolveMissingAssemblies;

        public override PortableExecutableReference ResolveMissingAssembly(MetadataReference definition, AssemblyIdentity referenceIdentity)
        {
            return _metadataReferenceResolver.ResolveMissingAssembly(definition, referenceIdentity);
        }


        public override ImmutableArray<PortableExecutableReference> ResolveReference(string reference, string baseFilePath, MetadataReferenceProperties properties)
        {
            if (reference.StartsWith("nuget:", StringComparison.OrdinalIgnoreCase))
            {
                // HACK We need to return something here to "mark" the reference as resolved. 
                // https://github.com/dotnet/roslyn/blob/master/src/Compilers/Core/Portable/ReferenceManager/CommonReferenceManager.Resolution.cs#L838
                return ImmutableArray<PortableExecutableReference>.Empty.Add(
                    MetadataReference.CreateFromFile(typeof(NuGetMetadataReferenceResolver).GetTypeInfo().Assembly.Location));
            }
            var resolvedReference = _metadataReferenceResolver.ResolveReference(reference, baseFilePath, properties);
            return resolvedReference;
        }
    }
}