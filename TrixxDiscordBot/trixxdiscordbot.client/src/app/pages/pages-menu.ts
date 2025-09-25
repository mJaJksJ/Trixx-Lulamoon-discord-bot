import { NbMenuItem } from '@nebular/theme';
import { Permission } from '../../api/models';

export interface TrixxMenuItem extends NbMenuItem {
  permission?: Permission;
  children?: TrixxMenuItem[];
}

// Если меняешь текст, не забудь поменять на бэке.
export const MENU_ITEMS: () => TrixxMenuItem[] = () => [
  {
    title: 'Главная',
    icon: 'monitor-outline',
    link: 'main',
    home: true,
    permission: Permission.MainPageRead,
  },
  {
    title: 'Мультфильмы',
    icon: 'film-outline',
    link: 'dictionary-cartoons',
    home: true,
    permission: Permission.DictionaryCartoonsRead,
  },
  {
    title: 'Студии',
    icon: 'cube-outline',
    link: 'dictionary-studios',
    home: true,
    permission: Permission.DictionaryStudiosRead,
  },
  {
    title: 'Пользователи',
    icon: 'people-outline',
    link: 'users',
    home: true,
    permission: Permission.TrixxUsersRead,
  },
];
