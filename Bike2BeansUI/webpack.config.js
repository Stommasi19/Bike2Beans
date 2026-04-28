const path = require("node:path");
const HtmlWebpackPlugin = require("html-webpack-plugin");
require("dotenv").config();
const webpack = require("webpack");

const DEV_SERVER_PORT = Number.parseInt(process.env.PORT || "3000", 10);

function getRequiredClientEnv(isProduction) {
    const apiBaseUrl = process.env.API_BASE_URL?.trim();
    const mapboxAccessToken = process.env.MAPBOX_ACCESS_TOKEN?.trim();

    if (!isProduction) {
        return { apiBaseUrl, mapboxAccessToken };
    }

    const missingEnvVars = [
        ["API_BASE_URL", apiBaseUrl],
        ["MAPBOX_ACCESS_TOKEN", mapboxAccessToken],
    ]
        .filter(([, value]) => !value)
        .map(([name]) => name);

    if (missingEnvVars.length > 0) {
        throw new Error(
            `Missing required production environment variable(s): ${missingEnvVars.join(", ")}`
        );
    }

    return { apiBaseUrl, mapboxAccessToken };
}

module.exports = (_env, argv) => {
    const isProduction = argv.mode === "production";
    const { apiBaseUrl, mapboxAccessToken } = getRequiredClientEnv(isProduction);

    return {
        entry: path.resolve(__dirname, "src", "index.tsx"),
        output: {
            path: path.resolve(__dirname, "dist"),
            filename: "bundle.js",
            clean: true,
            publicPath: "/",
        },
        devServer: {
            allowedHosts: "all",
            host: "0.0.0.0",
            historyApiFallback: true,
            open: false,
            port: DEV_SERVER_PORT,
            hot: true,
        },
        module: {
            rules: [
                {
                    // Some ESM packages (including React Navigation) use extensionless internal imports.
                    // Webpack 5 can require fully specified paths for ESM; disable that requirement here.
                    test: /\.m?js$/,
                    resolve: {
                        fullySpecified: false,
                    },
                },
                {
                    test: /\.css$/i,
                    use: ["style-loader", "css-loader", "postcss-loader"],
                },
                {
                    test: /\.(png|jpe?g|gif|webp)$/i,
                    type: "asset/resource",
                },
                {
                    test: /\.(ts|tsx)$/,
                    exclude: /node_modules/,
                    use: {
                        loader: "ts-loader",
                        options: {
                            configFile: "tsconfig.web.json",
                            transpileOnly: true,
                        },
                    },
                },
                {
                    test: /\.(js|jsx)$/,
                    exclude: /node_modules/,
                    use: {
                        loader: "babel-loader",
                        options: {
                            presets: ["@babel/preset-env", "@babel/preset-react"],
                        },
                    },
                },
            ],
        },
        resolve: {
            extensions: [
                ".web.tsx",
                ".web.ts",
                ".web.js",
                ".tsx",
                ".ts",
                ".mjs",
                ".js",
                ".jsx",
            ],
            alias: {
                "react-native$": "react-native-web",
                "@": path.resolve(__dirname, "src"),
            },
        },
        plugins: [
            new HtmlWebpackPlugin({
                template: path.resolve(__dirname, "public", "index.html"),
            }),
            new webpack.DefinePlugin({
                "process.env.API_BASE_URL": JSON.stringify(apiBaseUrl),
                "process.env.MAPBOX_ACCESS_TOKEN": JSON.stringify(mapboxAccessToken),
            }),
        ],
    };
};
