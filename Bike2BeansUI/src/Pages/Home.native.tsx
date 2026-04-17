import { useNavigation } from "@react-navigation/native";
import type { NativeStackNavigationProp } from "@react-navigation/native-stack";
import React, { useEffect, useState } from "react";
import { Pressable, ScrollView, Text, View } from "react-native";
import { SafeAreaView } from "react-native-safe-area-context";
import { GetCoffeeShops } from "../Api/Coffeeshops";
import { NavBar } from "../Components/NavBar.native";
import { CoffeeShopCard } from "../Features/CoffeeShop/CoffeeShopCards.native";
import type { CoffeeshopDto } from "../Data/CoffeeshopDto";
import type { RootStackParamList } from "../Navigation/types";
import { nativeStyles } from "../theme/nativeStyles";

type Navigation = NativeStackNavigationProp<RootStackParamList, "Home">;

export function Home() {
    const [shops, setShops] = useState<CoffeeshopDto[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [activeId, setActiveId] = useState<string | null>(null);
    const nav = useNavigation<Navigation>();

    useEffect(() => {
        let active = true;
        setLoading(true);
        setError(null);

        GetCoffeeShops()
            .then((response) => {
                if (!active) {
                    return;
                }
                setShops(response);
            })
            .catch((err) => {
                console.warn("Failed to load coffee shops", err);
                if (active) {
                    setError("Unable to load coffee shops right now.");
                }
            })
            .finally(() => {
                if (active) {
                    setLoading(false);
                }
            });

        return () => {
            active = false;
        };
    }, []);

    return (
        <SafeAreaView style={nativeStyles.screen}>
            <NavBar />
            <ScrollView contentContainerStyle={nativeStyles.pageShell}>
                <View style={nativeStyles.panel}>
                    <Text style={nativeStyles.sectionTitle}>Route Planning</Text>
                    <Text style={nativeStyles.muted}>
                        Native shell is now running without CSS/DOM dependencies. Map and drag/drop can be added as
                        dedicated native components next.
                    </Text>
                    <View style={nativeStyles.buttonRow}>
                        <Pressable
                            onPress={() => nav.navigate("RouteSetup")}
                            style={({ pressed }) => [
                                nativeStyles.buttonBase,
                                nativeStyles.buttonPrimary,
                                pressed && nativeStyles.buttonPressed,
                            ]}
                        >
                            <Text style={nativeStyles.buttonTextPrimary}>Route Setup</Text>
                        </Pressable>
                        <Pressable
                            onPress={() => nav.navigate("SavedRoutes")}
                            style={({ pressed }) => [
                                nativeStyles.buttonBase,
                                nativeStyles.buttonSecondary,
                                pressed && nativeStyles.buttonPressed,
                            ]}
                        >
                            <Text style={nativeStyles.buttonTextSecondary}>Saved Routes</Text>
                        </Pressable>
                    </View>
                </View>

                <View style={nativeStyles.panel}>
                    <Text style={nativeStyles.sectionTitle}>Coffee Shops</Text>
                    <View style={nativeStyles.statusRow}>
                        <Text style={nativeStyles.muted}>
                            {loading ? "Loading..." : `${shops.length} loaded`}
                        </Text>
                        <View style={nativeStyles.statusBadge}>
                            <Text style={nativeStyles.statusBadgeText}>API: /api/coffeeshops</Text>
                        </View>
                    </View>

                    {error ? (
                        <View style={nativeStyles.emptyState}>
                            <Text style={nativeStyles.muted}>{error}</Text>
                        </View>
                    ) : null}

                    {!loading && !error && shops.length === 0 ? (
                        <View style={nativeStyles.emptyState}>
                            <Text style={nativeStyles.muted}>
                                No coffee shops were returned. This is usually data-shape or empty-database related.
                            </Text>
                        </View>
                    ) : null}

                    {shops.map((shop) => (
                        <CoffeeShopCard
                            key={shop.id}
                            shop={shop}
                            active={shop.id === activeId}
                            onSelect={() => setActiveId(shop.id)}
                        />
                    ))}
                </View>
            </ScrollView>
        </SafeAreaView>
    );
}
