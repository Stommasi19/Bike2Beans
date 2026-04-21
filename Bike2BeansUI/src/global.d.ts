declare module "*.css" {
    const content: string;
    export default content;
}

declare const process: {
    env: {
        MAPBOX_ACCESS_TOKEN?: string;
    };
};
