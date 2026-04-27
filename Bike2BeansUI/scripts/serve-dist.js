const fs = require("node:fs");
const http = require("node:http");
const path = require("node:path");

const DIST_DIR = path.resolve(__dirname, "..", "dist");
const INDEX_FILE = path.join(DIST_DIR, "index.html");
const PORT = Number.parseInt(process.env.PORT || "3000", 10);
const HOST = process.env.HOST || "0.0.0.0";

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

    if (method === "HEAD") {
        response.writeHead(200, { "Content-Type": contentType });
        response.end();
        return;
    }

    const stream = fs.createReadStream(filePath);
    stream.on("open", () => {
        response.writeHead(200, { "Content-Type": contentType });
    });
    stream.on("error", (error) => {
        console.error(`Failed to read ${filePath}:`, error);
        if (!response.headersSent) {
            response.writeHead(500, { "Content-Type": "text/plain; charset=utf-8" });
        }
        response.end("Internal Server Error");
    });
    stream.pipe(response);
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
    if (requestUrl.pathname === "/health") {
        response.writeHead(200, { "Content-Type": "application/json; charset=utf-8" });
        response.end(JSON.stringify({ status: "ok" }));
        return;
    }

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

if (!Number.isInteger(PORT) || PORT < 1 || PORT > 65535) {
    console.error(`Invalid PORT value: ${process.env.PORT}`);
    process.exit(1);
}

if (!fs.existsSync(INDEX_FILE)) {
    console.error(`Missing production bundle: ${INDEX_FILE}`);
    console.error("Run `npm run build` before starting the production server.");
    process.exit(1);
}

server.listen(PORT, HOST, () => {
    console.log(`Serving Bike2BeansUI from ${DIST_DIR} on ${HOST}:${PORT}`);
});

server.on("error", (error) => {
    console.error(`Failed to start Bike2BeansUI on ${HOST}:${PORT}:`, error);
    process.exit(1);
});
