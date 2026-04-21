import { Pressable, Text, View } from "react-native";
import type { CoffeeshopDto } from "../../Data/CoffeeshopDto";
import { nativeStyles } from "../../theme/nativeStyles";

type Props = {
    shop: CoffeeshopDto;
    active?: boolean;
    onSelect?: () => void;
};

export function CoffeeShopCard({ shop, active = false, onSelect }: Props) {
    return (
        <Pressable onPress={onSelect} style={({ pressed }) => [pressed && nativeStyles.buttonPressed]}>
            <View style={[nativeStyles.shopCard, active && nativeStyles.shopCardActive]}>
                <Text style={nativeStyles.shopCardTitle}>{shop.name}</Text>
                <Text style={nativeStyles.shopCardMeta}>
                    {"★ "}
                    {(shop.rating ?? 0).toFixed(1)}
                    {"  ("}
                    {shop.userRatingsTotal ?? 0}
                    {")"}
                </Text>
                <Text style={nativeStyles.shopCardMeta}>{shop.address || "Address unavailable"}</Text>
            </View>
        </Pressable>
    );
}
