using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Schema;

namespace XSLTTransformer.Core
{
    /// <summary>
    /// GİB e-fatura ve e-arşiv şema kütüphanesi.
    /// Şema validasyonu ve e-belge standartlarına göre doğrulama sağlar.
    /// </summary>
    public sealed class GibSchemaValidator
    {
        private readonly XmlSchemaSet _schemaSet;
        private readonly Dictionary<string, string> _knownSchemas;

        public GibSchemaValidator()
        {
            _schemaSet = new XmlSchemaSet();
            _knownSchemas = new Dictionary<string, string>
            {
                ["efatura"] = "Schemas/eFatura.xsd",
                ["earsiv"] = "Schemas/eArsiv.xsd"
            };
            LoadSchemas();
        }

        private void LoadSchemas()
        {
            foreach (var path in _knownSchemas.Values)
            {
                if (File.Exists(path))
                    _schemaSet.Add(null, path);
            }
        }

        public bool Validate(string xml, string schemaType, out IList<string> errors)
        {
            errors = new List<string>();
            if (!_knownSchemas.TryGetValue(schemaType.ToLower(), out var schemaPath))
            {
                errors.Add("Bilinmeyen şema tipi.");
                return false;
            }

            var settings = new XmlReaderSettings
            {
                ValidationType = ValidationType.Schema,
                Schemas = _schemaSet
            };
            var errList = errors;
            settings.ValidationEventHandler += (s, e) => errList.Add(e.Message);

            using var reader = XmlReader.Create(new StringReader(xml), settings);
            try
            {
                while (reader.Read()) { }
                return errors.Count == 0;
            }
            catch (XmlException ex)
            {
                errors.Add(ex.Message);
                return false;
            }
        }
    }
}