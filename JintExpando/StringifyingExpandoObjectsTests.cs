using System.Collections.Generic;
using System.Dynamic;
using Jint;
using NUnit.Framework;

namespace JintExpando
{
    [TestFixture]
    public class StringifyingExpandoObjectsTests
    {
        [Test]
        public void Engine_should_stringify_an_ExpandoObject_correctly()
        {
            var engine = new Engine();

            dynamic expando = new ExpandoObject();
            expando.foo = 5;
            expando.bar = "A string";
            engine.SetValue(nameof(expando), expando);

            var result = ExecuteAndGetResult($"JSON.stringify({nameof(expando)})", engine);

            Assert.That(result, Is.EqualTo("{\"foo\":5,\"bar\":\"A string\"}"));
        }

        [Test]
        public void Engine_should_stringify_a_DictionaryOfStringAndObject_correctly()
        {
            var engine = new Engine();

            var dictionary = new Dictionary<string,object> {
                { "foo", 5 },
                { "bar", "A string"},
            };
            engine.SetValue(nameof(dictionary), dictionary);

            var result = ExecuteAndGetResult($"JSON.stringify({nameof(dictionary)})", engine);

            Assert.That(result, Is.EqualTo("{\"foo\":5,\"bar\":\"A string\"}"));
        }

        [Test]
        public void Engine_should_roundtrip_parsed_JSON_back_to_string_correctly()
        {
            var engine = new Engine();

            var json = "{\"foo\":5,\"bar\":\"A string\"}";
            var parsed = ExecuteAndGetResult($"JSON.parse('{json}')", engine);
            engine.SetValue(nameof(parsed), parsed);
            
            var result = ExecuteAndGetResult($"JSON.stringify({nameof(parsed)})", engine);

            Assert.That(result, Is.EqualTo(json));
        }


        object ExecuteAndGetResult(string expression, Engine engine)
        {
            engine.Execute(expression);
            return engine.GetCompletionValue().ToObject();
        }
    }
}