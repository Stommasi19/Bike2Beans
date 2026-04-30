const fs = require("node:fs");
const http = require("node:http");
const path = require("node:path");

const DIST_DIR = path.resolve(__dirname, "..", "dist");
const INDEX_FILE = path.join(DIST_DIR, "index.html");
const PORT = Number.parseInt(process.env.PORT || "3000", 10);

const MIME_TYPES = {
    ".css": "text/css; charset=utf-8",
    ".gif": "image/gif",
    ".html": "text/html; charset=utf-8",
    ".ico": "image/x-icon",
    ".jpeg": "image/jpeg",
    ".jpg": "image/jpeg",
    ".js": "text/javascript; charset=utf-8",
    ".json": "application/json; charset=utf-8",
    ".map": "application/json; charset=utf-8",
    ".png": "image/png",
    ".svg": "image/svg+xml; charset=utf-8",
    ".txt": "text/plain; charset=utf-8",
    ".webp": "image/webp",
};

function sendFile(response, filePath, method) {
    const extension = path.extname(filePath).toLowerCase();
    const contentType = MIME_TYPES[extension] || "application/octet-stream";

    response.writeHead(200, { "Content-Type": contentType });

    if (method === "HEAD") {
        response.end();
        return;
    }

    fs.createReadStream(filePath).pipe(response);
}

function sendNotFound(response) {
    response.writeHead(404, { "Content-Type": "text/plain; charset=utf-8" });
    response.end("Not Found");
}

function toSafeAssetPath(urlPathname) {
    const decodedPath = decodeURIComponent(urlPathname);
    const normalizedPath = path.posix.normalize(decodedPath);
    const relativePath = normalizedPath.replace(/^\/+/, "");
    const assetPath = path.resolve(DIST_DIR, relativePath);

    if (!assetPath.startsWith(DIST_DIR)) {
        return null;
    }

    return assetPath;
}

const server = http.createServer((request, response) => {
    const method = request.method || "GET";
    if (method !== "GET" && method !== "HEAD") {
        response.writeHead(405, { "Content-Type": "text/plain; charset=utf-8" });
        response.end("Method Not Allowed");
        return;
    }

    const requestUrl = new URL(request.url || "/", "http://127.0.0.1");
    const assetPath = toSafeAssetPath(requestUrl.pathname);
    if (!assetPath) {
        sendNotFound(response);
        return;
    }

    if (fs.existsSync(assetPath) && fs.statSync(assetPath).isFile()) {
        sendFile(response, assetPath, method);
        return;
    }

    if (path.extname(requestUrl.pathname)) {
        sendNotFound(response);
        return;
    }

    sendFile(response, INDEX_FILE, method);
});

server.listen(PORT, "0.0.0.0", () => {
    console.log(`Serving Bike2BeansUI from ${DIST_DIR} on port ${PORT}`);
});
