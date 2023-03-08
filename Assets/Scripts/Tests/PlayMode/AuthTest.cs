using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
public class AuthTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void AuthTestSimplePasses()
    {
        // Use the Assert class to test conditions
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator AuthTestWithEnumeratorPasses()
    {
        var gameObject = new GameObject();
        var firebase = gameObject.AddComponent<FirebaseAuthManager>();
        // Arrange
        var name = "John Doe";
        var email = "johndoe@example.com";
        var password = "password";
        var confirmPassword = "notmatchingpassword";

        // Act
        var coroutine = firebase.RegisterTestAsync(name, email, password, confirmPassword);
        yield return coroutine;

        // Assert
        LogAssert.Expect(LogType.Error, "Password does not match");
    }
}
