import { Injectable } from "@nestjs/common";
import { InjectRepository } from "@nestjs/typeorm";
import { UserEntity } from "@entities/user.entity";
import { FindOptionsWhere, Repository } from "typeorm";
import { CreateUserDtoWithoutAvatar } from "@contracts/dto/user/create-user.dto";
import { UserAlreadyExists } from "@exceptions/user/user-already-exists";
import { FilesService } from "@features/files/files.service";
import { join } from "node:path";
import { MEDIA_FOLDER_NAME } from "@constants/default.constant";
import {
  GetUserDto,
  GetUserDtoWithPassword,
} from "@contracts/dto/user/get-user.dto";
import { UpdateUserDtoWithoutAvatar } from "@contracts/dto/user/update-user.dto";
import { ColumnMetadata } from "typeorm/metadata/ColumnMetadata";
import { UserNotFoundException } from "@exceptions/user/user-not-found.exception";
import { PasswordService } from "@features/password/password.service";

@Injectable()
export class UsersService {
  constructor(
    @InjectRepository(UserEntity)
    private readonly repository: Repository<UserEntity>,
    private readonly filesService: FilesService,
    private readonly passwordService: PasswordService,
  ) {}

  public async getByEmail(
    email: string,
    notFoundError: boolean = false,
    withPassword: boolean = false,
  ): Promise<GetUserDto | GetUserDtoWithPassword | null> {
    const user: GetUserDto | GetUserDtoWithPassword = await this.getBy(
      { email },
      withPassword,
    );
    if (user === null && notFoundError) {
      throw new UserNotFoundException("email", email);
    }
    return user;
  }

  public async getById(
    id: number,
    notFoundError: boolean = false,
    withPassword: boolean = false,
  ): Promise<GetUserDto | GetUserDtoWithPassword | null> {
    const user: GetUserDto | GetUserDtoWithPassword = await this.getBy(
      { id },
      withPassword,
    );
    if (user === null && notFoundError) {
      throw new UserNotFoundException("id", id);
    }
    return user;
  }

  public async create(
    user: CreateUserDtoWithoutAvatar,
    avatar?: Express.Multer.File,
  ): Promise<GetUserDto> {
    const candidate: GetUserDto | null = await this.getByEmail(user.email);
    if (candidate !== null) {
      throw new UserAlreadyExists("email", user.email);
    }

    const newUser: GetUserDto = this.repository.create({
      ...user,
      password: await this.passwordService.hash(user.password),
    });
    await this.repository.save(newUser);
    if (avatar) {
      newUser.avatarPath = await this.uploadAvatar(avatar, `${newUser.id}`);
    }
    await this.repository.save(newUser);
    delete newUser["password"];
    return newUser;
  }

  public async delete(id: number): Promise<GetUserDto> {
    const user: GetUserDto = await this.getById(id, true);
    await this.filesService.delete(join("users", `${user.id}`), false);
    await this.repository.delete({ id });
    return user;
  }

  public async update(
    id: number,
    newest: UpdateUserDtoWithoutAvatar,
    avatar?: Express.Multer.File,
  ): Promise<GetUserDto> {
    let candidate: GetUserDtoWithPassword | null = (await this.getById(
      id,
      true,
      true,
    )) as GetUserDtoWithPassword;
    if (newest.password) {
      newest.password = await this.passwordService.hash(newest.password);
    }
    candidate = this.repository.merge(candidate as UserEntity, newest);
    if (avatar) {
      candidate.avatarPath = await this.uploadAvatar(avatar, `${candidate.id}`);
    }
    try {
      await this.repository.save(candidate);
    } catch {
      throw new UserAlreadyExists("email", candidate.email);
    }
    return (await this.getById(id, true)) as GetUserDto;
  }

  private async uploadAvatar(
    avatar: Express.Multer.File,
    userFolder: string,
  ): Promise<string> {
    return await this.filesService.upload(
      avatar,
      join("users", userFolder, MEDIA_FOLDER_NAME),
      "avatar",
      true,
    );
  }

  private async getBy(
    findOptions: FindOptionsWhere<UserEntity>,
    withPassword: boolean = false,
  ): Promise<GetUserDto | null> {
    return await this.repository.findOne({
      where: findOptions,
      select: this.getColumnsName(withPassword),
    });
  }

  private getColumnsName(withPassword: boolean = false): (keyof UserEntity)[] {
    const password: keyof UserEntity = "password";
    let columns: (keyof UserEntity)[] = this.repository.metadata.columns.map(
      (col: ColumnMetadata): string => col.propertyName,
    ) as (keyof UserEntity)[];
    if (withPassword === false) {
      columns = columns.filter((col: string): boolean => col !== password);
    }
    return columns;
  }
}
