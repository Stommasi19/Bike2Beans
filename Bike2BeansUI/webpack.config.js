const path = require("path");
const HtmlWebpackPlugin = require("html-webpack-plugin");
require("dotenv").config();
const webpack = require("webpack");


module.exports = {
    mode: "development",
    entry: path.resolve(__dirname, "src", "index.tsx"),
    output: {
        path: path.resolve(__dirname, "dist"),
        filename: "bundle.js",
        clean: true,
        publicPath: "/",
    },
    devServer: {
        port: 3000,
        historyApiFallback: true,
        open: true,
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
            "process.env.MAPBOX_ACCESS_TOKEN": JSON.stringify(process.env.MAPBOX_ACCESS_TOKEN),
        }),
    ],
};
