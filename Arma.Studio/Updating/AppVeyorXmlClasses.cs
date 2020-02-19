using System.Collections.Generic;
using System.Xml.Serialization;

namespace Arma.Studio.Updating
{
	[XmlRoot(ElementName = "AuthorEmail", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
	public class AuthorEmail
	{
		[XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
		public string Nil { get; set; }
	}

	[XmlRoot(ElementName = "Committed", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
	public class Committed
	{
		[XmlElement(ElementName = "DateTime", Namespace = "http://schemas.datacontract.org/2004/07/System")]
		public string DateTime { get; set; }
		[XmlElement(ElementName = "OffsetMinutes", Namespace = "http://schemas.datacontract.org/2004/07/System")]
		public string OffsetMinutes { get; set; }
		[XmlAttribute(AttributeName = "d4p1", Namespace = "http://www.w3.org/2000/xmlns/")]
		public string D4p1 { get; set; }
	}

	[XmlRoot(ElementName = "CommitterEmail", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
	public class CommitterEmail
	{
		[XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
		public string Nil { get; set; }
	}

	[XmlRoot(ElementName = "Created", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
	public class Created
	{
		[XmlElement(ElementName = "DateTime", Namespace = "http://schemas.datacontract.org/2004/07/System")]
		public string DateTime { get; set; }
		[XmlElement(ElementName = "OffsetMinutes", Namespace = "http://schemas.datacontract.org/2004/07/System")]
		public string OffsetMinutes { get; set; }
		[XmlAttribute(AttributeName = "d4p1", Namespace = "http://www.w3.org/2000/xmlns/")]
		public string D4p1 { get; set; }
		[XmlAttribute(AttributeName = "d3p1", Namespace = "http://www.w3.org/2000/xmlns/")]
		public string D3p1 { get; set; }
	}

	[XmlRoot(ElementName = "Finished", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
	public class Finished
	{
		[XmlElement(ElementName = "DateTime", Namespace = "http://schemas.datacontract.org/2004/07/System")]
		public string DateTime { get; set; }
		[XmlElement(ElementName = "OffsetMinutes", Namespace = "http://schemas.datacontract.org/2004/07/System")]
		public string OffsetMinutes { get; set; }
		[XmlAttribute(AttributeName = "d4p1", Namespace = "http://www.w3.org/2000/xmlns/")]
		public string D4p1 { get; set; }
	}

	[XmlRoot(ElementName = "MessageExtended", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
	public class MessageExtended
	{
		[XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
		public string Nil { get; set; }
	}

	[XmlRoot(ElementName = "PullRequestHeadBranch", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
	public class PullRequestHeadBranch
	{
		[XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
		public string Nil { get; set; }
	}

	[XmlRoot(ElementName = "PullRequestHeadCommitId", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
	public class PullRequestHeadCommitId
	{
		[XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
		public string Nil { get; set; }
	}

	[XmlRoot(ElementName = "PullRequestHeadRepository", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
	public class PullRequestHeadRepository
	{
		[XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
		public string Nil { get; set; }
	}

	[XmlRoot(ElementName = "PullRequestId", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
	public class PullRequestId
	{
		[XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
		public string Nil { get; set; }
	}

	[XmlRoot(ElementName = "PullRequestName", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
	public class PullRequestName
	{
		[XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
		public string Nil { get; set; }
	}

	[XmlRoot(ElementName = "Started", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
	public class Started
	{
		[XmlElement(ElementName = "DateTime", Namespace = "http://schemas.datacontract.org/2004/07/System")]
		public string DateTime { get; set; }
		[XmlElement(ElementName = "OffsetMinutes", Namespace = "http://schemas.datacontract.org/2004/07/System")]
		public string OffsetMinutes { get; set; }
		[XmlAttribute(AttributeName = "d4p1", Namespace = "http://www.w3.org/2000/xmlns/")]
		public string D4p1 { get; set; }
	}

	[XmlRoot(ElementName = "Tag", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
	public class Tag
	{
		[XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
		public string Nil { get; set; }
	}

	[XmlRoot(ElementName = "Updated", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
	public class Updated
	{
		[XmlElement(ElementName = "DateTime", Namespace = "http://schemas.datacontract.org/2004/07/System")]
		public string DateTime { get; set; }
		[XmlElement(ElementName = "OffsetMinutes", Namespace = "http://schemas.datacontract.org/2004/07/System")]
		public string OffsetMinutes { get; set; }
		[XmlAttribute(AttributeName = "d4p1", Namespace = "http://www.w3.org/2000/xmlns/")]
		public string D4p1 { get; set; }
		[XmlAttribute(AttributeName = "d3p1", Namespace = "http://www.w3.org/2000/xmlns/")]
		public string D3p1 { get; set; }
	}

	[XmlRoot(ElementName = "BuildModel", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
	public class BuildModel
	{
		[XmlElement(ElementName = "AuthorEmail", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public AuthorEmail AuthorEmail { get; set; }
		[XmlElement(ElementName = "AuthorName", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public string AuthorName { get; set; }
		[XmlElement(ElementName = "AuthorUsername", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public string AuthorUsername { get; set; }
		[XmlElement(ElementName = "Branch", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public string Branch { get; set; }
		[XmlElement(ElementName = "BuildId", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public int BuildId { get; set; }
		[XmlElement(ElementName = "BuildNumber", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public int BuildNumber { get; set; }
		[XmlElement(ElementName = "CommitId", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public string CommitId { get; set; }
		[XmlElement(ElementName = "Committed", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public Committed Committed { get; set; }
		[XmlElement(ElementName = "CommitterEmail", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public CommitterEmail CommitterEmail { get; set; }
		[XmlElement(ElementName = "CommitterName", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public string CommitterName { get; set; }
		[XmlElement(ElementName = "CommitterUsername", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public string CommitterUsername { get; set; }
		[XmlElement(ElementName = "Created", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public Created Created { get; set; }
		[XmlElement(ElementName = "Finished", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public Finished Finished { get; set; }
		[XmlElement(ElementName = "IsTag", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public string IsTag { get; set; }
		[XmlElement(ElementName = "Jobs", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public string Jobs { get; set; }
		[XmlElement(ElementName = "Message", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public string Message { get; set; }
		[XmlElement(ElementName = "MessageExtended", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public MessageExtended MessageExtended { get; set; }
		[XmlElement(ElementName = "Messages", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public string Messages { get; set; }
		[XmlElement(ElementName = "ProjectId", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public string ProjectId { get; set; }
		[XmlElement(ElementName = "PullRequestHeadBranch", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public PullRequestHeadBranch PullRequestHeadBranch { get; set; }
		[XmlElement(ElementName = "PullRequestHeadCommitId", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public PullRequestHeadCommitId PullRequestHeadCommitId { get; set; }
		[XmlElement(ElementName = "PullRequestHeadRepository", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public PullRequestHeadRepository PullRequestHeadRepository { get; set; }
		[XmlElement(ElementName = "PullRequestId", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public PullRequestId PullRequestId { get; set; }
		[XmlElement(ElementName = "PullRequestName", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public PullRequestName PullRequestName { get; set; }
		[XmlElement(ElementName = "Started", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public Started Started { get; set; }
		[XmlElement(ElementName = "Status", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public string Status { get; set; }
		[XmlElement(ElementName = "Tag", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public Tag Tag { get; set; }
		[XmlElement(ElementName = "Updated", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public Updated Updated { get; set; }
		[XmlElement(ElementName = "Version", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public string Version { get; set; }
	}

	[XmlRoot(ElementName = "Builds", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
	public class Builds
	{
		[XmlElement(ElementName = "BuildModel", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public List<BuildModel> BuildModel { get; set; }
	}

	[XmlRoot(ElementName = "AccessRights", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
	public class AccessRights
	{
		[XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
		public string Nil { get; set; }
		[XmlElement(ElementName = "AceAccessRight", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public List<AceAccessRight> AceAccessRight { get; set; }
	}

	[XmlRoot(ElementName = "BuildPriority", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
	public class BuildPriority
	{
		[XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
		public string Nil { get; set; }
	}

	[XmlRoot(ElementName = "BuildTimeout", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
	public class BuildTimeout
	{
		[XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
		public string Nil { get; set; }
	}

	[XmlRoot(ElementName = "CommitStatusContextName", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
	public class CommitStatusContextName
	{
		[XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
		public string Nil { get; set; }
	}

	[XmlRoot(ElementName = "Configuration", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
	public class Configuration
	{
		[XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
		public string Nil { get; set; }
	}

	[XmlRoot(ElementName = "CurrentBuild", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
	public class CurrentBuild
	{
		[XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
		public string Nil { get; set; }
	}

	[XmlRoot(ElementName = "CurrentBuildId", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
	public class CurrentBuildId
	{
		[XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
		public string Nil { get; set; }
	}

	[XmlRoot(ElementName = "CustomYmlName", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
	public class CustomYmlName
	{
		[XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
		public string Nil { get; set; }
	}

	[XmlRoot(ElementName = "DefaultBranch", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
	public class DefaultBranch
	{
		[XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
		public string Nil { get; set; }
	}

	[XmlRoot(ElementName = "IgnoreAppveyorYml", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
	public class IgnoreAppveyorYml
	{
		[XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
		public string Nil { get; set; }
	}

	[XmlRoot(ElementName = "NextBuildNumber", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
	public class NextBuildNumber
	{
		[XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
		public string Nil { get; set; }
	}

	[XmlRoot(ElementName = "NuGetFeed", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
	public class NuGetFeed
	{
		[XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
		public string Nil { get; set; }
	}

	[XmlRoot(ElementName = "RepositoryAuthentication", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
	public class RepositoryAuthentication
	{
		[XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
		public string Nil { get; set; }
	}

	[XmlRoot(ElementName = "RepositoryBranch", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
	public class RepositoryBranch
	{
		[XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
		public string Nil { get; set; }
	}

	[XmlRoot(ElementName = "RepositoryUsername", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
	public class RepositoryUsername
	{
		[XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
		public string Nil { get; set; }
	}

	[XmlRoot(ElementName = "ScheduleCrontabExpression", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
	public class ScheduleCrontabExpression
	{
		[XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
		public string Nil { get; set; }
	}

	[XmlRoot(ElementName = "AceAccessRightDefinition", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
	public class AceAccessRightDefinition
	{
		[XmlElement(ElementName = "Description", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public string Description { get; set; }
		[XmlElement(ElementName = "Name", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public string Name { get; set; }
	}

	[XmlRoot(ElementName = "AccessRightDefinitions", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
	public class AccessRightDefinitions
	{
		[XmlElement(ElementName = "AceAccessRightDefinition", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public List<AceAccessRightDefinition> AceAccessRightDefinition { get; set; }
	}

	[XmlRoot(ElementName = "AceAccessRight", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
	public class AceAccessRight
	{
		[XmlElement(ElementName = "Name", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public string Name { get; set; }
		[XmlElement(ElementName = "Allowed", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public Allowed Allowed { get; set; }
	}

	[XmlRoot(ElementName = "RoleAce", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
	public class RoleAce
	{
		[XmlElement(ElementName = "AccessRights", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public AccessRights AccessRights { get; set; }
		[XmlElement(ElementName = "IsAdmin", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public string IsAdmin { get; set; }
		[XmlElement(ElementName = "Name", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public string Name { get; set; }
		[XmlElement(ElementName = "RoleId", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public string RoleId { get; set; }
	}

	[XmlRoot(ElementName = "Allowed", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
	public class Allowed
	{
		[XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
		public string Nil { get; set; }
	}

	[XmlRoot(ElementName = "RoleAces", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
	public class RoleAces
	{
		[XmlElement(ElementName = "RoleAce", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public List<RoleAce> RoleAce { get; set; }
	}

	[XmlRoot(ElementName = "SecurityDescriptor", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
	public class SecurityDescriptor
	{
		[XmlElement(ElementName = "AccessRightDefinitions", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public AccessRightDefinitions AccessRightDefinitions { get; set; }
		[XmlElement(ElementName = "RoleAces", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public RoleAces RoleAces { get; set; }
	}

	[XmlRoot(ElementName = "SshPublicKey", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
	public class SshPublicKey
	{
		[XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
		public string Nil { get; set; }
	}

	[XmlRoot(ElementName = "StatusBadgeId", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
	public class StatusBadgeId
	{
		[XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
		public string Nil { get; set; }
	}

	[XmlRoot(ElementName = "VersionFormat", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
	public class VersionFormat
	{
		[XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
		public string Nil { get; set; }
	}

	[XmlRoot(ElementName = "WebhookId", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
	public class WebhookId
	{
		[XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
		public string Nil { get; set; }
	}

	[XmlRoot(ElementName = "WebhookUrl", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
	public class WebhookUrl
	{
		[XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
		public string Nil { get; set; }
	}

	[XmlRoot(ElementName = "Project", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
	public class Project
	{
		[XmlElement(ElementName = "AccessRights", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public AccessRights AccessRights { get; set; }
		[XmlElement(ElementName = "AccountId", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public string AccountId { get; set; }
		[XmlElement(ElementName = "AccountName", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public string AccountName { get; set; }
		[XmlElement(ElementName = "AlwaysBuildClosedPullRequests", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public string AlwaysBuildClosedPullRequests { get; set; }
		[XmlElement(ElementName = "BuildPriority", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public BuildPriority BuildPriority { get; set; }
		[XmlElement(ElementName = "BuildTimeout", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public BuildTimeout BuildTimeout { get; set; }
		[XmlElement(ElementName = "Builds", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public string Builds { get; set; }
		[XmlElement(ElementName = "CommitStatusContextName", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public CommitStatusContextName CommitStatusContextName { get; set; }
		[XmlElement(ElementName = "Configuration", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public Configuration Configuration { get; set; }
		[XmlElement(ElementName = "Created", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public Created Created { get; set; }
		[XmlElement(ElementName = "CurrentBuild", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public CurrentBuild CurrentBuild { get; set; }
		[XmlElement(ElementName = "CurrentBuildId", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public CurrentBuildId CurrentBuildId { get; set; }
		[XmlElement(ElementName = "CustomYmlName", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public CustomYmlName CustomYmlName { get; set; }
		[XmlElement(ElementName = "DefaultBranch", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public DefaultBranch DefaultBranch { get; set; }
		[XmlElement(ElementName = "DisablePullRequestWebhooks", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public string DisablePullRequestWebhooks { get; set; }
		[XmlElement(ElementName = "DisablePushWebhooks", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public string DisablePushWebhooks { get; set; }
		[XmlElement(ElementName = "EnableDeploymentInPullRequests", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public string EnableDeploymentInPullRequests { get; set; }
		[XmlElement(ElementName = "EnableSecureVariablesInPullRequests", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public string EnableSecureVariablesInPullRequests { get; set; }
		[XmlElement(ElementName = "EnableSecureVariablesInPullRequestsFromSameRepo", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public string EnableSecureVariablesInPullRequestsFromSameRepo { get; set; }
		[XmlElement(ElementName = "IgnoreAppveyorYml", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public IgnoreAppveyorYml IgnoreAppveyorYml { get; set; }
		[XmlElement(ElementName = "IsGitHubApp", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public string IsGitHubApp { get; set; }
		[XmlElement(ElementName = "IsPrivate", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public string IsPrivate { get; set; }
		[XmlElement(ElementName = "Name", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public string Name { get; set; }
		[XmlElement(ElementName = "NextBuildNumber", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public NextBuildNumber NextBuildNumber { get; set; }
		[XmlElement(ElementName = "NuGetFeed", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public NuGetFeed NuGetFeed { get; set; }
		[XmlElement(ElementName = "ProjectId", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public string ProjectId { get; set; }
		[XmlElement(ElementName = "RepositoryAuthentication", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public RepositoryAuthentication RepositoryAuthentication { get; set; }
		[XmlElement(ElementName = "RepositoryBranch", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public RepositoryBranch RepositoryBranch { get; set; }
		[XmlElement(ElementName = "RepositoryName", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public string RepositoryName { get; set; }
		[XmlElement(ElementName = "RepositoryScm", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public string RepositoryScm { get; set; }
		[XmlElement(ElementName = "RepositoryType", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public string RepositoryType { get; set; }
		[XmlElement(ElementName = "RepositoryUsername", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public RepositoryUsername RepositoryUsername { get; set; }
		[XmlElement(ElementName = "RollingBuilds", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public string RollingBuilds { get; set; }
		[XmlElement(ElementName = "RollingBuildsDoNotCancelRunningBuilds", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public string RollingBuildsDoNotCancelRunningBuilds { get; set; }
		[XmlElement(ElementName = "RollingBuildsOnlyForPullRequests", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public string RollingBuildsOnlyForPullRequests { get; set; }
		[XmlElement(ElementName = "SaveBuildCacheInPullRequests", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public string SaveBuildCacheInPullRequests { get; set; }
		[XmlElement(ElementName = "ScheduleCrontabExpression", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public ScheduleCrontabExpression ScheduleCrontabExpression { get; set; }
		[XmlElement(ElementName = "SecurityDescriptor", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public SecurityDescriptor SecurityDescriptor { get; set; }
		[XmlElement(ElementName = "SkipBranchesWithoutAppveyorYml", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public string SkipBranchesWithoutAppveyorYml { get; set; }
		[XmlElement(ElementName = "Slug", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public string Slug { get; set; }
		[XmlElement(ElementName = "SshPublicKey", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public SshPublicKey SshPublicKey { get; set; }
		[XmlElement(ElementName = "StatusBadgeId", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public StatusBadgeId StatusBadgeId { get; set; }
		[XmlElement(ElementName = "Tags", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public string Tags { get; set; }
		[XmlElement(ElementName = "Updated", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public Updated Updated { get; set; }
		[XmlElement(ElementName = "VersionFormat", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public VersionFormat VersionFormat { get; set; }
		[XmlElement(ElementName = "WebhookId", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public WebhookId WebhookId { get; set; }
		[XmlElement(ElementName = "WebhookUrl", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public WebhookUrl WebhookUrl { get; set; }
	}

	[XmlRoot(ElementName = "ProjectHistoryResults", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
	public class ProjectHistoryResults
	{
		[XmlElement(ElementName = "Builds", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public Builds Builds { get; set; }
		[XmlElement(ElementName = "Project", Namespace = "http://schemas.datacontract.org/2004/07/Appveyor.Models")]
		public Project Project { get; set; }
		[XmlAttribute(AttributeName = "i", Namespace = "http://www.w3.org/2000/xmlns/")]
		public string I { get; set; }
		[XmlAttribute(AttributeName = "xmlns")]
		public string Xmlns { get; set; }
	}


}
