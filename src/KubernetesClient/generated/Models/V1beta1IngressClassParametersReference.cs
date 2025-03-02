// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace k8s.Models
{
    using Microsoft.Rest;
    using Newtonsoft.Json;
    using System.Linq;

    /// <summary>
    /// IngressClassParametersReference identifies an API object. This can be
    /// used to specify a cluster or namespace-scoped resource.
    /// </summary>
    public partial class V1beta1IngressClassParametersReference
    {
        /// <summary>
        /// Initializes a new instance of the
        /// V1beta1IngressClassParametersReference class.
        /// </summary>
        public V1beta1IngressClassParametersReference()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the
        /// V1beta1IngressClassParametersReference class.
        /// </summary>
        /// <param name="kind">Kind is the type of resource being
        /// referenced.</param>
        /// <param name="name">Name is the name of resource being
        /// referenced.</param>
        /// <param name="apiGroup">APIGroup is the group for the resource being
        /// referenced. If APIGroup is not specified, the specified Kind must
        /// be in the core API group. For any other third-party types, APIGroup
        /// is required.</param>
        /// <param name="namespaceProperty">Namespace is the namespace of the
        /// resource being referenced. This field is required when scope is set
        /// to "Namespace" and must be unset when scope is set to
        /// "Cluster".</param>
        /// <param name="scope">Scope represents if this refers to a cluster or
        /// namespace scoped resource. This may be set to "Cluster" (default)
        /// or "Namespace". Field can be enabled with
        /// IngressClassNamespacedParams feature gate.</param>
        public V1beta1IngressClassParametersReference(string kind, string name, string apiGroup = default(string), string namespaceProperty = default(string), string scope = default(string))
        {
            ApiGroup = apiGroup;
            Kind = kind;
            Name = name;
            NamespaceProperty = namespaceProperty;
            Scope = scope;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// Gets or sets aPIGroup is the group for the resource being
        /// referenced. If APIGroup is not specified, the specified Kind must
        /// be in the core API group. For any other third-party types, APIGroup
        /// is required.
        /// </summary>
        [JsonProperty(PropertyName = "apiGroup")]
        public string ApiGroup { get; set; }

        /// <summary>
        /// Gets or sets kind is the type of resource being referenced.
        /// </summary>
        [JsonProperty(PropertyName = "kind")]
        public string Kind { get; set; }

        /// <summary>
        /// Gets or sets name is the name of resource being referenced.
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets namespace is the namespace of the resource being
        /// referenced. This field is required when scope is set to "Namespace"
        /// and must be unset when scope is set to "Cluster".
        /// </summary>
        [JsonProperty(PropertyName = "namespace")]
        public string NamespaceProperty { get; set; }

        /// <summary>
        /// Gets or sets scope represents if this refers to a cluster or
        /// namespace scoped resource. This may be set to "Cluster" (default)
        /// or "Namespace". Field can be enabled with
        /// IngressClassNamespacedParams feature gate.
        /// </summary>
        [JsonProperty(PropertyName = "scope")]
        public string Scope { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            if (Kind == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "Kind");
            }
            if (Name == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "Name");
            }
        }
    }
}
