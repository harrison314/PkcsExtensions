var PkcsExtensionsBlazor;
(function (PkcsExtensionsBlazor) {
    function toBase64(uint8Array) {
        return window.btoa(uint8Array.reduce(function (data, byte) { return data + String.fromCharCode(byte); }, ''));
    }
    var getRandomValues = function (size) {
        var array = new Uint8Array(size);
        crypto.getRandomValues(array);
        return toBase64(array);
    };
    var generateKeyRsa = function (modulusLength, name) {
        if (name === void 0) { name = 'RSA-OAEP'; }
        return crypto.subtle.generateKey({
            name: name,
            modulusLength: modulusLength,
            publicExponent: new Uint8Array([0x01, 0x00, 0x01]),
            hash: { name: 'SHA-256' },
        }, true, ['encrypt', 'decrypt'])
            .then(function (t) { return crypto.subtle.exportKey('pkcs8', t.privateKey); })
            .then(function (buffer) { return toBase64(new Uint8Array(buffer)); });
    };
    var mapEcJwk = function (jwk) {
        return {
            kty: 'EC',
            crv: jwk.crv,
            d: jwk.d,
            x: jwk.x,
            y: jwk.y
        };
    };
    var generateKeyEcdsa = function (namedCurve) {
        var ecdsaParams = {
            name: 'ECDSA',
            namedCurve: namedCurve
        };
        return crypto.subtle.generateKey(ecdsaParams, true, ['sign', 'verify'])
            .then(function (t) { return crypto.subtle.exportKey('jwk', t.privateKey); })
            .then(function (t) { return mapEcJwk(t); });
    };
    function Load() {
        window['PkcsExtensionsBlazor_getRandomValues'] = getRandomValues;
        window['PkcsExtensionsBlazor_generateKeyRsa'] = generateKeyRsa;
        window['PkcsExtensionsBlazor_generateKeyEcdsa'] = generateKeyEcdsa;
    }
    PkcsExtensionsBlazor.Load = Load;
})(PkcsExtensionsBlazor || (PkcsExtensionsBlazor = {}));
PkcsExtensionsBlazor.Load();
