import { StyleSheet } from "react-native";
import { nativeTheme } from "./nativeTheme";

const { colors, radius } = nativeTheme;

export const nativeStyles = StyleSheet.create({
    screen: {
        flex: 1,
        backgroundColor: colors.uiBg,
    },
    pageShell: {
        paddingHorizontal: 16,
        paddingVertical: 16,
        gap: 12,
    },
    panel: {
        borderRadius: radius.panel,
        borderWidth: 1,
        borderColor: colors.uiPanelBorder,
        backgroundColor: colors.uiPanelBg,
        padding: 16,
        gap: 8,
    },
    sectionTitle: {
        color: colors.uiText,
        fontSize: 20,
        fontWeight: "700",
    },
    muted: {
        color: colors.uiMuted,
        fontSize: 14,
        lineHeight: 20,
    },
    statusRow: {
        flexDirection: "row",
        alignItems: "center",
        justifyContent: "space-between",
        marginBottom: 4,
    },
    statusBadge: {
        borderRadius: radius.pill,
        borderWidth: 1,
        borderColor: colors.uiInputBorder,
        backgroundColor: colors.uiInputBg,
        paddingHorizontal: 10,
        paddingVertical: 4,
    },
    statusBadgeText: {
        color: colors.uiMuted,
        fontSize: 12,
        fontWeight: "600",
    },
    navbar: {
        borderBottomWidth: 1,
        borderBottomColor: colors.uiNavbarBorder,
        backgroundColor: colors.uiNavbarBg,
        paddingHorizontal: 12,
        paddingVertical: 10,
    },
    navbarList: {
        flexDirection: "row",
        gap: 8,
    },
    navbarButton: {
        borderRadius: radius.pill,
        paddingHorizontal: 14,
        paddingVertical: 8,
        borderWidth: 1,
        borderColor: colors.uiInputBorder,
        backgroundColor: colors.uiInputBg,
    },
    navbarButtonPressed: {
        backgroundColor: colors.brand100,
    },
    navbarButtonText: {
        color: colors.uiLink,
        fontSize: 14,
        fontWeight: "600",
    },
    authScreen: {
        flex: 1,
        alignItems: "center",
        justifyContent: "center",
        padding: 20,
        backgroundColor: colors.uiBg,
    },
    authCard: {
        width: "100%",
        maxWidth: 560,
        borderRadius: radius.card,
        borderWidth: 1,
        borderColor: colors.uiInputBorder,
        backgroundColor: colors.uiInputBg,
        padding: 20,
        gap: 12,
    },
    authCardLogin: {
        maxWidth: 500,
    },
    authCardSignup: {
        maxWidth: 560,
    },
    authTitle: {
        color: colors.uiText,
        fontSize: 28,
        lineHeight: 34,
        fontWeight: "700",
        marginBottom: 6,
    },
    fieldGroup: {
        gap: 6,
    },
    fieldLabel: {
        color: colors.uiText,
        fontSize: 14,
        fontWeight: "600",
        marginLeft: 2,
    },
    input: {
        borderWidth: 1,
        borderColor: colors.uiInputBorder,
        backgroundColor: colors.uiInputBg,
        color: colors.uiText,
        borderRadius: radius.input,
        paddingHorizontal: 12,
        paddingVertical: 10,
        fontSize: 14,
    },
    buttonRow: {
        flexDirection: "row",
        flexWrap: "wrap",
        gap: 12,
        marginTop: 8,
    },
    buttonBase: {
        borderRadius: 10,
        paddingHorizontal: 14,
        paddingVertical: 10,
        borderWidth: 1,
        alignItems: "center",
        justifyContent: "center",
        minWidth: 120,
    },
    buttonPressed: {
        opacity: 0.9,
    },
    buttonPrimary: {
        backgroundColor: colors.brand700,
        borderColor: colors.brand700,
    },
    buttonSecondary: {
        backgroundColor: colors.uiInputBorder,
        borderColor: colors.uiInputBorder,
    },
    buttonGhost: {
        backgroundColor: colors.white,
        borderColor: colors.uiInputBorder,
        alignSelf: "flex-start",
    },
    buttonTextPrimary: {
        color: colors.white,
        fontSize: 14,
        fontWeight: "600",
    },
    buttonTextSecondary: {
        color: colors.uiText,
        fontSize: 14,
        fontWeight: "600",
    },
    buttonTextGhost: {
        color: colors.uiText,
        fontSize: 14,
        fontWeight: "600",
    },
    shopCard: {
        borderRadius: 12,
        borderWidth: 1,
        borderColor: colors.uiInputBorder,
        backgroundColor: colors.white,
        padding: 12,
        gap: 4,
        marginTop: 8,
    },
    shopCardActive: {
        borderColor: colors.latte400,
        backgroundColor: colors.latte50,
    },
    shopCardTitle: {
        color: colors.uiText,
        fontSize: 16,
        fontWeight: "700",
    },
    shopCardMeta: {
        color: colors.uiMuted,
        fontSize: 13,
        lineHeight: 18,
    },
    emptyState: {
        borderRadius: 12,
        borderWidth: 1,
        borderColor: colors.uiInputBorder,
        backgroundColor: colors.uiInputBg,
        padding: 14,
        marginTop: 8,
    },
});
