using Firebase.Firestore;
[FirestoreData]
public struct User
{
    [FirestoreProperty]
    public string UserID { get; set; }
    [FirestoreProperty]
    public string UserName { get; set; }
    [FirestoreProperty]
    public int HighScore { get; set; }
}

