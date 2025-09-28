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
    permission: Permission.MainPage_Read,
  },
  {
    title: 'Мультфильмы',
    icon: 'film-outline',
    link: 'dictionary-cartoons',
    home: true,
    permission: Permission.DictionaryCartoons_Read,
  },
  {
    title: 'Студии',
    icon: 'cube-outline',
    link: 'dictionary-studios',
    home: true,
    permission: Permission.DictionaryStudios_Read,
  },
  {
    title: 'Пользователи',
    icon: 'people-outline',
    link: 'users',
    home: true,
    permission: Permission.TrixxUsers_Read,
  },
  {
    title: 'Роли',
    icon: 'color-palette-outline',
    link: 'roles',
    home: true,
    permission: Permission.TrixxUsers_Read,
  },
];
