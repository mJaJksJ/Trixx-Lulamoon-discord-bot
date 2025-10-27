import { CartoonType } from "../../../api/models";

// export const cartoonTypesLabels: { [x in keyof typeof CartoonType]-?: string } = {
//     FeatureLengthFilm: 'Полнометражный фильм',
//     ShortFilm: 'Короткометражный фильм',
//     SerialFilm: 'Мультсериал',
// }

export const cartoonTypesShortLabels: { [x in keyof typeof CartoonType]-?: string } = {
    FeatureLengthFilm: 'п/м',
    ShortFilm: 'к/м',
    SerialFilm: 'м/с',
}