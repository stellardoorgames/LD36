using UnityEngine;
using System.Collections;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Collections.Generic;


[JsonObject("Holder")]
public class CharacterHolder
{
	[JsonProperty("Character")]
	public CharacterData character { get; set;}
}

[JsonObject("Character")]
public class CharacterData
{
	[JsonProperty("name")]
	public string name { get; set;}
	[JsonProperty("ego")]
	public float ego {get; set;}
	[JsonProperty("damage")]
	public float damage {get; set;}
	[JsonProperty("intro")]
	public string[] intros { get; set;}
	[JsonProperty("attacks")]
	public AttackData[] attacks { get; set;}
	[JsonProperty("conversations")]
	public TierData[] conversations { get; set;}
	[JsonProperty("items")]
	public ItemConversationData[] items { get; set;}
	[JsonProperty("win")]
	public string[] win { get; set;}
	[JsonProperty("lose")]
	public string[] lose { get; set;}

}

[JsonObject("Attack")]
public class AttackData
{
	[JsonProperty("name")]
	public string name { get; set;}
	[JsonProperty("damage")]
	public float damage { get; set;}
	[JsonProperty("dialog")]
	public string[] dialog { get; set;}
}

[JsonObject("Tier")]
public class TierData
{
	[JsonProperty("prompt")]
	public string prompt { get; set;}
	[JsonProperty("replies")]
	public ReplyData[] replies { get; set;}
}

[JsonObject("Reply")]
public class ReplyData
{
	[JsonProperty("text")]
	public string text { get; set;}
	[JsonProperty("damage")]
	public float damage { get; set;}
	[JsonProperty("progress")]
	public bool progress { get; set;}
	[JsonProperty("followup")]
	public string[] followup { get; set;}
}

[JsonObject("Item")]
public class ItemConversationData
{
	[JsonProperty("damage")]
	public float damage { get; set;}
	[JsonProperty("item")]
	public string item { get; set;}
	[JsonProperty("replies")]
	public string[] replies { get; set;}
}


