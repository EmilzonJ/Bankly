using Domain.Base;
using MongoDB.Bson;

namespace Domain.Collections;

/// <summary>
/// Collection to save the counter of the accounts and
/// to generate unique account numbers based on the sequence.
/// </summary>
public class AccountSequence : CollectionBase
{
    public const string CollectionName = "AccountSequence";
    public static readonly ObjectId SequenceId = ObjectId.Parse("666dd7289c5c68e2987b1de1");

    public int Seq { get; set; }
}
