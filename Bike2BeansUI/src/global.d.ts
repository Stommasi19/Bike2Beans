declare module "*.css" {
    const content: string;
    export default content;
}

declare const process: {
    env: {
        API_BASE_URL?: string;
        MAPBOX_ACCESS_TOKEN?: string;
    };
};
